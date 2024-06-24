using System.ComponentModel.DataAnnotations;

namespace OnlineRailwayReservation.DTO
{
    public class TrainDto
    {
        public int Train_Id { get; set; }
       
        public string Train_Name { get; set; }
       
        public string Train_Number { get; set; }
       
        public int Total_seats { get; set; }
        public int Available_seats { get; set; }
        public int Available_seats_General { get; set; }
        public int Available_seats_Ladies { get; set; }
        
        public int SourceStationId { get; set; }
        
        public int DestinationStationId { get; set; }
        
        public decimal Fare { get; set; }
       
        public DateTime ArrivalTime { get; set; }
      
        public DateTime DepartureTime { get; set; }
    }
}
