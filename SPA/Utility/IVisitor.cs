namespace SPA.Utility;

public interface IVisitor<in T>
{
    void Visit(T element);
}