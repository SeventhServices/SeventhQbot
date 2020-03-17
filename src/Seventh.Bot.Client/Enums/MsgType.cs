namespace Seventh.Bot.Client.Enums
{

    /// <summary>
    /// Friend = 1, Group = 2, TemporarilyGroup = 3,
    /// Discussion = 4, TemporarilyDiscussion = 5,
    /// Temporarily = 6
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// Friend
        /// </summary>
        Friend = 1,

        /// <summary>
        /// Group
        /// </summary>
        Group = 2,

        /// <summary>
        /// TemporarilyGroup
        /// </summary>
        TemporarilyGroup = 3,

        /// <summary>
        /// Discussion
        /// </summary>
        Discussion = 4,

        /// <summary>
        /// TemporarilyDiscussion
        /// </summary>
        TemporarilyDiscussion = 5,

        /// <summary>
        /// Temporarily
        /// </summary>
        Temporarily = 6
    }

    public enum MpqMsgType
    {
        Friend = 0,
        Group = 1,
        Discussion = 2,
        TemporarilyGroup = 3,
        TemporarilyDiscussion = 4
    }
}