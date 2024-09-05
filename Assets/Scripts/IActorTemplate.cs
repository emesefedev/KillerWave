public interface IActorTemplate
{
    int SendDamage();
    void TakeDamage(int damage);
    void Die();
    void ActorStats(SOActorModel actorModel);
}
