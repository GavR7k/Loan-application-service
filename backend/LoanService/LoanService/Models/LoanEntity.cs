namespace LoanService.Models
{
    public class LoanEntity
    {
        //Уникальный индентификатор
        public int Id { get; set; }

        //Статус заявки
        public LoanStatus Status { get; set; }

        //Номер заявки
        public string Number { get; set; }

        //Сумма займа
        public decimal Amount { get; set; }

        //Срок займа (в днях)
        public int TermValue { get; set; }

        //Процентная ставка
        public decimal InterestValue { get; set; }

        //Дата и время создания 
        public DateTimeOffset CreatedAt { get; set; }

        //Дата и время обновления
        public DateTimeOffset ModifiedAt { get; set; }
    }

    public enum LoanStatus
    {
        Published = 1,
        Unpublished = 0
    }

}
