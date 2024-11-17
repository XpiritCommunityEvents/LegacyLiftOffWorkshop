namespace Project_HMS.Entities;

public class Booking
{
    public int BId { get; set; }
    public int RId { get; set; }
    public string CheckIn { get; set; }
    public string CheckOut { get; set; }
    public int FId { get; set; }
    public int SId { get; set; }
    public string CName { get; set; }
    public string Address { get; set; }
    public string CPhone { get; set; }
    public string NID { get; set; }
    public double Total { get; set; }
    public double Advance { get; set; }
    public double Remaining { get; set; }
}
