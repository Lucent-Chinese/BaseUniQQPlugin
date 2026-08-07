using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using UniQQ.SDK;
using UniQQ.SDK.Builders;
using UniQQ.SDK.Events.MessageEvents;
using UniQQ.SDK.Events.Meta;
using UniQQ.SDK.Events.Notice;
using UniQQ.SDK.Events.Request;
using UniQQ.SDK.Interfaces;
using UniQQ.SDK.Models;
using UniQQ.SDK.Models.Segments;
using UniQQ.SDK.Plugins;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace $safeprojectname$
{
    /// <summary>
    /// ===================== 【UniQQ 插件开发模板 - 通用规范说明】 =====================
    /// 【模板用途】UniQQ SDK 插件开发基础模板，包含完整生命周期、全事件示例、SDK API 调用参考
    /// 【开发必看注意事项】
    /// 1. 插件必须实现 PluginBase 接口，生命周期顺序：Load → OnEnable → 运行事件 → OnDisable → Unload
    /// 2. 所有事件订阅在 OnEnable 中注册，必须在 OnDisable 中反订阅，防止内存泄漏、重复触发事件
    /// 3. 事件处理方法统一使用 async Task，耗时操作(网络请求、文件IO)禁止同步阻塞
    /// 4. 禁止硬编码本地绝对路径(如 D:\xxx)，文件存储优先使用 Context.DataPath 插件专属数据目录
    /// 5. MessageBuilder 为消息构造工具类，支持文本、@人、图片、表情、音乐、分享等富消息
    /// 6. IPluginContext 是插件核心上下文，包含所有机器人操作 API、事件总线、路径、清单信息
    /// 7. 本模板中所有【API示例】均为注释状态，开发者按需取消注释使用
    /// ==============================================================================
    /// </summary>

    public class $safeprojectname$ : PluginBase
    {
        // ========== 插件元信息【开发者按需修改】 ==========
        /// <summary>
        /// 插件名称，插件管理器中展示
        /// </summary>
        public override string Name => "插件模板";
        /// <summary>
        /// 插件版本号，建议遵循 主版本.次版本.修订号 格式
        /// </summary>
        public override string Version => "1.0.0";

        // ========== 插件私有字段【开发者按需扩展】 ==========
        /// <summary>
        /// 当前插件唯一ID，从插件清单获取
        /// </summary>
        private string _pluginId = string.Empty;
        /// <summary>
        /// 插件专属数据目录，所有插件本地数据、配置、缓存建议存放在此
        /// 路径由框架分配，跨平台兼容，禁止手动写绝对路径
        /// </summary>
        private string _dataDirectory = string.Empty;

        // ========== 插件生命周期方法（固定顺序，请勿修改方法名） ==========

        public $safeprojectname$()
        {
            //请勿执行任何代码，初始化代码请在Load()函数下执行
            
        }

        /// <summary>
        /// 生命周期：插件被框架载入（最先执行，仅执行一次）
        /// 适用场景：全局初始化、静态资源加载、全局变量初始化
        /// </summary>
        /// <returns>异步任务</returns>
        public override Task Load()
        {
            // ！！！！！ 不建议在此函数进行窗口弹出的操作，需要弹出窗口请转到OnSettings()函数下  ！！！！！！
            // ！！！！！ 此方法下禁止调用Context！！！！！

            return Task.CompletedTask;
        }

        /// <summary>
        /// 生命周期：插件被启用（Load 之后执行，插件启动核心逻辑）
        /// 适用场景：初始化上下文字段、订阅各类机器人事件
        /// 重要：事件订阅统一写在此处
        /// </summary>
        /// <returns>异步任务</returns>
        public override Task OnEnable()
        {
            // ！！！！！ 不建议在此函数进行窗口弹出的操作，需要弹出窗口请转到OnSettings()函数下  ！！！！！！

            // 读取插件ID与数据目录
            _pluginId = Context.Manifest.Id;
            _dataDirectory = Context.DataPath;//插件的数据目录，插件的数据可以放在这里

            // ========== 订阅机器人事件 ==========
            //Context.Events.On<BotOnlineEvent>(OnBotOnline);//机器人上线事件
            //Context.Events.On<BotOfflineEvent>(OnBotOffline);//机器人下线通知
            //Context.Events.On<HeartbeatEvent>(OnHeartbeat);//机器人心跳事件
            Context.Events.On<GroupMessageEvent>(OnGroupMessage);//群消息事件
            //Context.Events.On<PrivateMessageEvent>(OnPrivateMessage);//私聊消息事件
            //Context.Events.On<TempMessageEvent>(OnTempMessage);//群临时会话消息事件
            //Context.Events.On<BotJoinGroupEvent>(OnBotJoinGroup);//机器人加入新群
            //Context.Events.On<EssenceMessageEvent>(OnEssenceMessage);//群消息精华事件
            //Context.Events.On<FileUploadEvent>(OnFileUpload);//群文件上传事件
            //Context.Events.On<ReactionEvent>(OnReaction);//表情回应事件
            //Context.Events.On<FriendAddedEvent>(OnFriendAdded);//新增好友事件
            //Context.Events.On<GroupPokeEvent>(OnGroupPoke);//群聊戳一戳事件
            //Context.Events.On<FriendPokeEvent>(OnFriendPoke);//好友戳一戳事件
            //Context.Events.On<GroupNameChangedEvent>(OnGroupNameChanged);//群名称变更事件
            //Context.Events.On<GroupMessageRecallEvent>(OnGroupMessageRecall);//群聊消息撤回事件
            //Context.Events.On<FriendMessageRecallEvent>(OnFriendMessageRecall);//好友消息撤回事件
            //Context.Events.On<FriendRequestEvent>(OnFriendRequest);//被加好友申请事件
            //Context.Events.On<GroupRequestEvent>(OnGroupRequest);//某人申请加群事件
            //Context.Events.On<BotGroupInviteEvent>(OnBotGroupInvite);//机器人被邀请加群事件
            //Context.Events.On<MemberJoinEvent>(OnMemberJoin);//群成员增加事件
            //Context.Events.On<BotLeaveGroupEvent>(OnBotLeaveGroup);//机器人主动退群事件
            //Context.Events.On<MemberLeaveEvent>(OnMemberLeave);//群成员退群事件
            //Context.Events.On<MemberKickEvent>(OnMemberKick);//群成员被踢事件
            //Context.Events.On<BotKickEvent>(OnBotKick);//机器人被踢事件
            //Context.Events.On<MemberMuteEvent>(OnMemberMute);//群成员被禁言事件
            //Context.Events.On<MemberUnmuteEvent>(OnMemberUnmute);//群成员被解除禁言事件
            //Context.Events.On<MemberSetAdminEvent>(OnMemberSetAdmin);//群成员被设置管理员事件
            //Context.Events.On<MemberUnsetAdminEvent>(OnMemberUnsetAdmin);//群成员被取消管理员事件

            Context.WriteLog($"[{_pluginId}] 已启用");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 生命周期：插件设置菜单被调用（用户点击插件的设置时执行）
        /// 适用场景：此处为插件窗口的入口，一般弹出插件的设置窗口/菜单窗口
        /// 核心要求：show窗口时不要执行耗时操作，避免用户产生误解导致重复操作
        /// </summary>
        /// <returns>异步任务</returns>
        public override Task OnSettings()
        {
            /// 适用场景：此处为插件窗口的入口，一般弹出插件的设置窗口/菜单窗口
            /// 核心要求：show窗口时不要执行耗时操作，避免用户产生误解导致重复操作


            return Task.CompletedTask;
        }

        /// <summary>
        /// 生命周期：插件被停用（插件关闭/禁用时执行）
        /// 核心要求：必须反订阅所有已注册事件，避免内存泄漏、事件重复执行
        /// </summary>
        /// <returns>异步任务</returns>
        public override Task OnDisable()
        {
            // ！！！！！ 不建议在此函数进行窗口弹出的操作，需要弹出窗口请转到OnSettings()函数下  ！！！！！！

            

            // 取消事件订阅，否则会造成内存泄漏和事件误发
            //Context.Events.Off<BotOnlineEvent>();
            //Context.Events.Off<BotOfflineEvent>();
            //Context.Events.Off<HeartbeatEvent>();
            Context.Events.Off<GroupMessageEvent>();
            //Context.Events.Off<PrivateMessageEvent>();
            //Context.Events.Off<TempMessageEvent>();
            //Context.Events.Off<BotJoinGroupEvent>();
            //Context.Events.Off<EssenceMessageEvent>();
            //Context.Events.Off<FileUploadEvent>();
            //Context.Events.Off<ReactionEvent>();
            //Context.Events.Off<FriendAddedEvent>();
            //Context.Events.Off<GroupPokeEvent>();
            //Context.Events.Off<FriendPokeEvent>();
            //Context.Events.Off<GroupNameChangedEvent>();
            //Context.Events.Off<GroupMessageRecallEvent>();
            //Context.Events.Off<FriendMessageRecallEvent>();
            //Context.Events.Off<FriendRequestEvent>();
            //Context.Events.Off<GroupRequestEvent>();
            //Context.Events.Off<BotGroupInviteEvent>();
            //Context.Events.Off<MemberJoinEvent>();
            //Context.Events.Off<BotLeaveGroupEvent>();
            //Context.Events.Off<MemberLeaveEvent>();
            //Context.Events.Off<MemberKickEvent>();
            //Context.Events.Off<BotKickEvent>();
            //Context.Events.Off<MemberMuteEvent>();
            //Context.Events.Off<MemberUnmuteEvent>();
            //Context.Events.Off<MemberSetAdminEvent>();
            //Context.Events.Off<MemberUnsetAdminEvent>();
            return Task.CompletedTask;
        }

        /// <summary>
        /// 生命周期：插件被彻底卸载（框架销毁插件时执行）
        /// 适用场景：释放全局资源、关闭网络连接、保存最终数据
        /// </summary>
        /// <returns>异步任务</returns>
        public override Task Unload()
        {
            // ！！！！！ 不建议在此函数进行窗口弹出的操作，需要弹出窗口请转到OnSettings()函数下  ！！！！！！

            


            return Task.CompletedTask;
        }

        // ========== 事件处理方法 + 字段说明 + 示例 ==========

        /// <summary>
        /// 事件：私聊消息事件
        /// 触发时机：机器人收到私聊消息时
        /// </summary>
        /// <param name="e">私聊消息事件对象</param>
        /// <remarks>
        /// e.Bot_Id：当前机器人QQ号
        /// e.User_Id：发送消息的好友QQ号
        /// e.Message：消息实体，RawText = 消息纯文本，Segments = 消息分段(图文/表情等)
        /// e.User_NickName：好友昵称
        /// </remarks>
        private async Task OnPrivateMessage(PrivateMessageEvent e)//私聊消息事件
        {
            var rawText = e.Message.RawText;

            // ===================== IPluginContext API 示例：发送私聊消息（注释） =====================
            /// <summary>
            /// 方法：SendPrivateMessageAsync
            /// 作用：向指定QQ发送私聊消息
            /// 参数1：botUin 机器人QQ
            /// 参数2：userId 接收方QQ
            /// 参数3：message 消息对象
            /// </summary>
            //await Context.SendPrivateMessageAsync(e.Bot_Id ,e.User_Id ,rawText);
            return;
        }

        /// <summary>
        /// 事件：群消息事件（最常用事件）
        /// 触发时机：任意群内发送消息时触发
        /// </summary>
        /// <param name="e">群消息事件对象</param>
        /// <remarks>
        /// e.Bot_Id：当前机器人QQ号
        /// e.Group_Id：消息所属群号
        /// e.User_Id：消息发送者QQ号
        /// e.User_NickName：发送者昵称
        /// e.User_Card：发送者群名片
        /// e.User_Role：群内身份(群主/管理/普通成员)
        /// e.MessageId：本条消息ID，用于撤回、引用回复
        /// e.Message.RawText：消息纯文本
        /// e.IsAtBot：是否@了本机器人（bool）
        /// e.ReplyAsync()：事件内置方法，快速引用回复本条消息
        /// </remarks>
        private async Task OnGroupMessage(GroupMessageEvent e)//群消息事件
        {
            var rawText = e.Message.RawText.TrimStart();//加去除收尾空格的写法
            await Context.WriteLog($"[{Name}] 收到群消息: {rawText} (来自群:{e.Group_Id}, 发送者:{e.User_Id})");//输出日志到框架UI

            //if (e.Message.RawText == "签到")
            //{
            //    await Context.SendGroupMessageAsync(e.Bot_Id, e.Group_Id, "签到了，下次早点来签到！" );//该api可以跨机器人/跨群发送消息
            //}
            //if (e.Message.RawText == "UniQQ")
            //{
            //    await e.ReplyAsync("叫我了！", true);//该api直接对收到的消息进行回复，参数Reference = true 时对收到的消息进行引用回复
            //}
            //if (e.IsAtBot)//判断是否at机器人
            //{
            //    await e.ReplyAsync(MessageBuilder.At(e.User_Id) + "艾特我了！" + MessageBuilder.Face(123));//MessageBuilder的拼接与调用
            //}
            //if (e.Message.RawText.Contains("上线", StringComparison.OrdinalIgnoreCase))//常用模糊问答
            //{
            //    await e.ReplyAsync("吹牛逼(来自UniQQ回复)");
            //}
            //if (e.Message.RawText == "给我个图片")
            //{
            //    await e.ReplyAsync(MessageBuilder.Image("D:\\.......\\logo48.png"));//MessageBuilder.Image本地图片调用
            //    await e.ReplyAsync(MessageBuilder.Image("http://.......png"));//MessageBuilder.Image 图片url调用
            //}

            // ===================== 【点歌功能示例】 =====================
            Regex _whiteSpaceRegex = new Regex(@"[\s\r\n]", RegexOptions.Compiled);
            int MaxSongNameLength = 30;
            if (rawText.Length >= 2 && rawText.Substring(0, 2) == "点歌")
            {
                try
                {
                    string rawSong = rawText.Substring(2);
                    if (rawSong.Length > MaxSongNameLength)
                        rawSong = rawSong.Substring(0, MaxSongNameLength);

                    string pureName = _whiteSpaceRegex.Replace(rawSong, "");
                    if (string.IsNullOrWhiteSpace(pureName))
                    {
                        await e.ReplyAsync("请输入有效的歌曲名称");
                        return;
                    }
                    var songId = await SearchSongIdAsync(pureName);
                    if (string.IsNullOrEmpty(songId))
                    {
                        await e.ReplyAsync("未找到该歌曲，请更换关键词重试");
                        return;
                    }

                    await e.ReplyAsync(MessageBuilder.NeteaseMusic(songId));
                }
                catch (Exception ex)
                {
                    await e.ReplyAsync($"查询歌曲服务暂时出错，请稍后重试{ex}");
                }
            }

            // ===================== 【MessageBuilder 全功能示例 - 注释状态】 =====================
            // MessageBuilder：消息分段构造器，拼接后可用于发送群/私聊消息
            /*
            // 1. 纯文本消息
            var textMsg = MessageBuilder.Text("这是纯文本消息");

            // 2. @ 指定用户
            var atMsg = MessageBuilder.At(12345678) + MessageBuilder.Text(" 你好呀");

            // 3. QQ表情 Face(表情ID)
            var faceMsg = MessageBuilder.Face(123);

            // 4. 图片消息(本地路径/网络URL)
            var imgMsg = MessageBuilder.Image("C:\\test.png");

            // 5. 语音消息
            var voiceMsg = MessageBuilder.Voice("C:\\test.amr");

            // 6. 视频消息
            var videoMsg = MessageBuilder.Video("C:\\test.mp4");

            // 7. 引用回复（Reply 传入消息ID）
            var replyMsg = MessageBuilder.Reply(e.MessageId) + MessageBuilder.Text("引用回复内容");

            // 8. 网易云音乐卡片
            var neteaseMusic = MessageBuilder.NeteaseMusic("歌曲ID");
            // 9. QQ音乐卡片
            var qqMusic = MessageBuilder.QQMusic("歌曲ID");
            // 10. 酷狗/酷我音乐卡片
            var kugouMusic = MessageBuilder.KugouMusic("歌曲ID");
            var kuwoMusic = MessageBuilder.KuwoMusic("歌曲ID");

            // 11. 自定义音乐卡片
            var customMusic = MessageBuilder.CustomMusic(
                title: "歌曲名",
                content: "歌手",
                image: "封面图URL",
                audio: "播放地址",
                url: "跳转链接"
            );

            // 12. 分享卡片
            var share = MessageBuilder.Share("分享标题", "跳转URL", "简介", "封面图URL");

            // 13. JSON / XML 卡片消息
            var jsonMsg = MessageBuilder.Json("{}");
            var xmlMsg = MessageBuilder.Xml("<xml></xml>");
            */

            // ===================== 【IPluginContext 常用API 示例 - 注释状态】 =====================
            /*
            // 1. 发送群消息（基础）
            await Context.SendGroupMessageAsync(e.Bot_Id, e.Group_Id, textMsg);

            // 2. 发送群临时会话消息
            await Context.SendGroupTempMessageAsync(e.Bot_Id, e.Group_Id, e.User_Id, textMsg);

            // 3. 撤回消息（传入消息ID）
            await Context.RecallMessageAsync(e.Bot_Id, e.MessageId);

            // 4. 禁言群成员（单位：秒，0=解除禁言）
            await Context.SetGroupMemberMuteAsync(e.Bot_Id, e.Group_Id, e.User_Id, 60);

            // 5. 全员禁言（true开启，false关闭）
            await Context.SetGroupWholeMuteAsync(e.Bot_Id, e.Group_Id, true);

            // 6. 设置/取消管理员
            await Context.SetGroupAdminAsync(e.Bot_Id, e.Group_Id, e.User_Id, true);

            // 7. 踢出群成员（第二个参数：是否拒绝再次加群）
            await Context.KickGroupMemberAsync(e.Bot_Id, e.Group_Id, e.User_Id, false);

            // 8. 获取群成员列表
            var memberList = await Context.GetGroupMemberListAsync(e.Bot_Id, e.Group_Id);

            // 9. 获取用户头像URL
            var avatarUrl = await Context.GetUserAvatarUrl(e.User_Id);

            // 10. 发送群公告
            await Context.SendGroupNoticeAsync(e.Bot_Id, e.Group_Id, "公告标题", "公告内容");

            // 11. 上传群文件
            await Context.UploadGroupFileAsync(e.Bot_Id, e.Group_Id, "C:\\test.txt", "展示文件名");
            */

            // 群聊消息处理逻辑
            return;
        }

        /// <summary>
        /// 事件：机器人上线事件
        /// 触发时机：机器人账号登录上线时
        /// </summary>
        /// <param name="e">上线事件对象</param>
        /// <remarks>
        /// e.BotUin：上线机器人QQ号
        /// e.Timestamp：事件时间戳
        /// </remarks>
        private async Task OnBotOnline(BotOnlineEvent e)
        {

            return;
        }

        /// <summary>
        /// 事件：机器人下线通知事件
        /// 触发时机：机器人离线、掉线、主动下线时
        /// </summary>
        /// <param name="e">下线事件对象</param>
        /// <remarks>
        /// e.BotUin：下线机器人QQ号
        /// e.Message：下线原因描述
        /// e.Timestamp：事件时间戳
        /// </remarks>
        private async Task OnBotOffline(BotOfflineEvent e)
        {

            return;
        }

        /// <summary>
        /// 事件：机器人心跳事件
        /// 触发时机：框架定时心跳（默认间隔由框架配置）
        /// 适用场景：定时任务、状态巡检、保活逻辑
        /// </summary>
        /// <param name="e">心跳事件对象</param>
        /// <remarks>
        /// e.BotUin：机器人QQ
        /// e.Timestamp：心跳时间
        /// e.Latency：心跳间隔
        /// e.Online：机器人是否在线
        /// </remarks>
        private async Task OnHeartbeat(HeartbeatEvent e)
        {

            return;
        }

        /// <summary>
        /// 事件：群临时会话消息事件
        /// 触发时机：非好友在群内点开机器人头像发起临时会话
        /// </summary>
        /// <param name="e">临时会话事件对象</param>
        /// <remarks>
        /// e.Bot_Id：机器人QQ
        /// e.Group_Id：来源群号
        /// e.User_Id：对方QQ
        /// e.Message：消息实体
        /// </remarks>
        private async Task OnTempMessage(TempMessageEvent e)
        {

            return;
        }

        /// <summary>
        /// 事件：机器人加入新群事件
        /// 触发时机：机器人被邀请/主动加入群聊
        /// </summary>
        /// <param name="e">入群事件对象</param>
        /// <remarks>
        /// e.Group_Id：群号
        /// e.Operator_Id：操作人QQ(邀请人)
        /// e.IsInvited：是否为被邀请入群
        /// </remarks>
        private async Task OnBotJoinGroup(BotJoinGroupEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：群消息精华事件
        /// 触发时机：消息被设为精华/取消精华
        /// </summary>
        /// <param name="e">精华消息事件</param>
        /// <remarks>
        /// e.MessageId：被操作的消息ID
        /// e.User_Id：消息发送者
        /// e.Operator_Id：设置精华的管理员
        /// e.IsAdd：true=设为精华，false=取消精华
        /// </remarks>
        private async Task OnEssenceMessage(EssenceMessageEvent e)
        { 

            return; 
        }

        /// <summary>
        /// 事件：群文件上传事件
        /// 触发时机：群内有人上传文件
        /// </summary>
        /// <param name="e">文件上传事件</param>
        /// <remarks>
        /// e.FileId：文件ID
        /// e.FileName：文件名
        /// e.FileSize：文件大小
        /// e.FileUrl：文件链接
        /// </remarks>
        private async Task OnFileUpload(FileUploadEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：表情回应(点赞/表情互动)事件
        /// 触发时机：群内对消息添加/取消表情回应
        /// </summary>
        private async Task OnReaction(ReactionEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：新增好友事件
        /// 触发时机：对方通过好友申请，成为机器人好友
        /// </summary>
        private async Task OnFriendAdded(FriendAddedEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：群聊戳一戳事件
        /// 触发时机：群内使用「戳一戳」功能
        /// </summary>
        /// <remarks>
        /// e.User_Id：发起戳一戳的人
        /// e.Target_Id：被戳的人
        /// </remarks>
        private async Task OnGroupPoke(GroupPokeEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：好友戳一戳事件
        /// 触发时机：私聊中使用戳一戳
        /// </summary>
        private async Task OnFriendPoke(FriendPokeEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：群名称变更事件
        /// 触发时机：群管理员修改群名
        /// </summary>
        /// <remarks>
        /// e.NewName：新群名
        /// e.Operator_Id：修改人QQ
        /// </remarks>
        private async Task OnGroupNameChanged(GroupNameChangedEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：群消息撤回事件
        /// 触发时机：群内撤回消息
        /// </summary>
        private async Task OnGroupMessageRecall(GroupMessageRecallEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：好友消息撤回事件
        /// 触发时机：私聊对方撤回消息
        /// </summary>
        private async Task OnFriendMessageRecall(FriendMessageRecallEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：被加好友申请事件
        /// 触发时机：有人向机器人发起好友申请
        /// </summary>
        /// <remarks>
        /// e.Flag：申请标识，用于处理同意/拒绝
        /// e.Comment：申请附言
        /// </remarks>
        private async Task OnFriendRequest(FriendRequestEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：加群申请事件
        /// 触发时机：有人申请加入本群
        /// </summary>
        private async Task OnGroupRequest(GroupRequestEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：机器人被邀请加群事件
        /// 触发时机：群主/管理邀请机器人入群
        /// </summary>
        private async Task OnBotGroupInvite(BotGroupInviteEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：群成员增加事件
        /// 触发时机：普通成员入群
        /// </summary>
        private async Task OnMemberJoin(MemberJoinEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：机器人主动退群事件
        /// 触发时机：机器人主动退出群聊
        /// </summary>
        private async Task OnBotLeaveGroup(BotLeaveGroupEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：群成员退群事件
        /// 触发时机：普通成员主动退群
        /// </summary>
        private async Task OnMemberLeave(MemberLeaveEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：群成员被踢事件
        /// 触发时机：管理员踢出普通成员
        /// </summary>
        private async Task OnMemberKick(MemberKickEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：机器人被踢事件
        /// 触发时机：机器人被群管理员踢出群
        /// </summary>
        private async Task OnBotKick(BotKickEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：群成员被禁言事件
        /// 触发时机：管理员对成员执行禁言
        /// </summary>
        /// <param name="e">禁言事件对象</param>
        /// <remarks>
        /// e.User_Id：被禁言人QQ
        /// e.Duration：禁言时长(秒)
        /// e.Operator_Id：执行禁言的管理员
        /// </remarks>
        private async Task OnMemberMute(MemberMuteEvent e)
        {
            if(e.User_Id != 0)
            {
                await Context.SendGroupMessageAsync(e.Bot_Id, e.Group_Id, $"{e.User_Id}被禁言了！禁言时长：{e.Duration}");
            }
            return; 
        }

        /// <summary>
        /// 事件：群成员被解除禁言事件
        /// 触发时机：管理员解除成员禁言
        /// </summary>
        private async Task OnMemberUnmute(MemberUnmuteEvent e)
        {
            if (e.User_Id != 0)
            {
                await Context.SendGroupMessageAsync(e.Bot_Id, e.Group_Id, $"{e.User_Id}被解除禁言了！");
            }
            
            return; 
        }

        /// <summary>
        /// 事件：群成员被设置管理员事件
        /// 触发时机：成员被任命为群管理
        /// </summary>
        private async Task OnMemberSetAdmin(MemberSetAdminEvent e)
        { 
            
            return; 
        }

        /// <summary>
        /// 事件：群成员被取消管理员事件
        /// 触发时机：撤销成员管理员权限
        /// </summary>
        private async Task OnMemberUnsetAdmin(MemberUnsetAdminEvent e)
        {

            return;
        }



        // ========== 模板插件中通过歌名取出网易云音乐的歌曲ID（点歌功能调用） ==========
        private async Task<string?> SearchSongIdAsync(string songName)
        {
            try
            {
                string url =
                    $"https://music-api.gdstudio.xyz/api.php?types=search&source=netease&name={Uri.EscapeDataString(songName)}";

                using var http = new HttpClient();

                string json = await http.GetStringAsync(url);

                var songs = JsonNode.Parse(json)?.AsArray();

                if (songs == null || songs.Count == 0)
                    return null;

                foreach (var song in songs)
                {
                    if (song?["name"]?.ToString() == songName)
                    {
                        return song["id"]?.ToString();
                    }
                }

                return songs[0]?["id"]?.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


    }
}
