﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model;

public  class Books
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Author { get; set; }
	public string Type { get; set; }
	public int Availability { get; set; }
	[Column("Rental date")]
	public DateTime? Rental_date { get; set; }
	[Column("Planned return date")]
	public DateTime? Planned_return_date { get; set; }
	public int SmsSendApi { get; set; }
	public int? ReaderID { get; set; }
	public int? Reservation {  get; set; }
	[Column("Reservation End")]
	public DateTime? Reservation_End {  get; set; }
	[Column("Reservation ReaderID")]
	public int? ReservationReaderID { get; set; }
}
