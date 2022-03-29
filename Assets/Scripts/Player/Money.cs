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
            throw new System.ArgumentException("��������� ������������� ���������� ����� ������");

        Value += count;
    }

    public void Pay(int count)
    {
        if (count < 0)
            throw new System.ArgumentException("��������� ������������� ���������� ����� ������");

        Value -= count;
    }
}
