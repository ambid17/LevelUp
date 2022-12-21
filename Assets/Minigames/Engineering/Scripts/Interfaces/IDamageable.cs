namespace Interfaces
{
    public interface IDamageable
    {
        public void ApplyDamage(int damage);
        public void Destroyed();
    }
}
