using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs;

public class Conversation
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConversationId { get; set; }
    public string ConservationName { get; set; }
    public int UserId { get; set; }

    public ICollection<Message> Messages { get; set; }
    public User User { get; set; }
}
