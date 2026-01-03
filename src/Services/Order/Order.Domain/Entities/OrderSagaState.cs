using BuildingBlocks.Core.Domain;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Entities;

public class OrderSagaState : Entity<Guid>, SagaStateMachineInstance
{
    // MassTransit Saga için zorunlu alan. Genelde OrderId ile aynı olur.
    public Guid CorrelationId { get; set; }

    // O anki durum (Örn: "StockReservationPending", "PaymentPending")
    public string CurrentState { get; set; } = default!;

    // Optimistic Concurrency için (Veritabanı çakışmalarını önler)
    public byte[] RowVersion { get; set; } = default!;

    // İsteğe bağlı: Süreç başladığında ve bittiğinde tarih tutmak için
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    // Saga sürecinde ihtiyaç duyulabilecek geçici veriler
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
}
