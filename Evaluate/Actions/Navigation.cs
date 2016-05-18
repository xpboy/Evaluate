using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluate.Actions
{
    public class Navigations
    {
        [Serializable]
        public class Nave
        {
            public int CareerId { get; set; }
            public int SubjectId { get; set; }
            public int QuestionId { get; set; }
            public int AnswerId { get; set; }
        }
    }
}