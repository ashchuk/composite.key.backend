namespace CompositeKey.Model
{
    public abstract class EntityBase<TKey> : EntityBase, IEntityBase<TKey>
    {
        public EntityBase() : base()
        {
        }

        public virtual TKey Id { get; set; }
    }
    public abstract class EntityBase : IEntityBase
    {
        public EntityBase()
        {
        }
    }
}
