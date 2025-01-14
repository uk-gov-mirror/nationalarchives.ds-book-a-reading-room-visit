﻿using book_a_reading_room_visit.model;
using System;
using System.Collections.Generic;

namespace book_a_reading_room_visit.web.Models
{
    public class AvailabilityViewModel
    {
        public BookingTypes BookingType { get; set; }
        public SeatTypes SeatType { get; set; }
        public List<AvailableSeat> AvailableBookings { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AvailableSeat
    {
        public DateTime Date { get; set; }
        public int AvailableSeats { get; set; }
    }
}
