using MagicDuel.Spells;

namespace MagicDuel
{
    public class DamageAmounts
    {
        public float health { get; private set; }
        public float mana { get; private set; }
        public float fire { get; private set; }
        public float ice { get; private set; }
        public float water { get; private set; }
        public float lightning { get; private set; }

        public DamageAmounts()
        {
            health = 0;
            mana = 0;
            fire = 0;
            ice = 0;
            water = 0;
            lightning = 0;
        }

        public DamageAmounts(DamageAmounts other)
        {
            health = other.health;
            mana = other.mana;
            fire = other.fire;
            ice = other.ice;
            water = other.water;
            lightning = other.lightning;
        }

        public DamageAmounts(float health, float mana, float fire, float ice, float water, float lightning)
        {
            this.health = health;
            this.mana = mana;
            this.fire = fire;
            this.ice = ice;
            this.water = water;
            this.lightning = lightning;
        }

        public float Get(DamageTypes type)
        {
            switch (type)
            {
                case DamageTypes.Health:
                    return health;

                case DamageTypes.Mana:
                    return mana;

                case DamageTypes.Fire:
                    return fire;

                case DamageTypes.Ice:
                    return ice;

                case DamageTypes.Water:
                    return water;

                case DamageTypes.Lightning:
                    return lightning;

                default:
                    throw new System.Exception("Unknown damage type " + type);
            }
        }

        public void Set(DamageTypes type, float value)
        {
            switch (type)
            {
                case DamageTypes.Health:
                    health = value;
                    break;

                case DamageTypes.Mana:
                    mana = value;
                    break;

                case DamageTypes.Fire:
                    fire = value;
                    break;

                case DamageTypes.Ice:
                    ice = value;
                    break;

                case DamageTypes.Water:
                    water = value;
                    break;

                case DamageTypes.Lightning:
                    lightning = value;
                    break;

                default:
                    throw new System.Exception("Unknown damage type " + type);
            }
        }

        public DamageAmounts GetMultiplied(DamageAmounts other)
        {
            return new DamageAmounts(health * other.health, mana * other.mana, fire * other.fire,
                ice * other.ice, water * other.water, lightning * other.lightning);
        }

        public DamageAmounts GetMultiplied(float amount)
        {
            return new DamageAmounts(health * amount, mana * amount, fire * amount, ice * amount,
                water * amount, lightning * amount);
        }
    }
}
