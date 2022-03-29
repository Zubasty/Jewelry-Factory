public class Money
{
    public int Value { get; private set; }

    public Money(int startValue)
    {
        Value = startValue;
    }

    public Money() : this(0) { }

    public void AddMoney(int count)
    {
        if (count < 0)
            throw new System.ArgumentException("ƒобавл€ть отрицательное количество денег нельз€");

        Value += count;
    }

    public void Pay(int count)
    {
        if (count < 0)
            throw new System.ArgumentException("«аплатить отрицательное количество денег нельз€");

        Value -= count;
    }
}
