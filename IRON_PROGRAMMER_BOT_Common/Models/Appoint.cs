using System;

namespace IRON_PROGRAMMER_BOT_Common.Models
{
    public class Appoint
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string DocName { get; set; }
        public string DocType { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }

        public override string ToString()
        {
            return $"{DocType} {DocName}\n{Date.ToString("dd.MM.yyyy HH:mm")}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Appoint appoint) 
                return Id == appoint.Id && UserId == appoint.UserId && Date == appoint.Date;
            return false;
        }

        public override int GetHashCode()
        {
            return $"{Id}{UserId}{DocName}{DocType}{Date}".GetHashCode();
        }

        public void NextMonth()
        {
            AddMonth(1);
        }

        public void PreviousMonth()
        {
            AddMonth(-1);
        }

        public void SetTime(TimeSpan time)
        {
            Date = Date.Add(time);
        }

        private void AddMonth(int months)
        {
            Date = Date.AddMonths(months);
        }
    }
}
