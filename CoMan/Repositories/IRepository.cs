using System.Linq.Expressions;

public interface IRepository<T> where T : EntityBase
{
    T GetById(int id);
    IEnumerable<T> List();
    IEnumerable<T> List(Expression<Func<T, bool>> predicate);
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);  
}

public abstract class EntityBase
{
    public int Id { get; protected set; }
}