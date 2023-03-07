using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime SubscribeDate { get; set; }
        public DateTime? UnsubscribeDate { get; set; }
        public string UnsubscribeCause { get; set; }
        public bool FlagBlockSub { get; set; }
        public string Notes { get; set; }

        public override string ToString()
        {
            return String.Format("{0,-5}{1,-30}{2,-20}{3,-20}{4,-30}{5,-5}{6,-30}",
                Id, Email, SubscribeDate, UnsubscribeDate, UnsubscribeCause, FlagBlockSub, Notes);
        }
    }
}
