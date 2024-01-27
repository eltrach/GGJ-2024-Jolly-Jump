namespace VTemplate
{
    public delegate void PerkLevelEvent(int level);


    public interface IPerk
    {
        /// <summary>
        /// Event you can subscribe to, it's called every time the value has changed, giving the new value as parameter
        /// </summary>
        event PerkLevelEvent OnPerkLevelChanged;

        /// <summary>
        /// Represents the name that you can use to display the perk, it is also used as key with the IDataManager
        /// Be careful to use different perk names if you use multiple perks
        /// </summary>
        string PerkName { get; }
        /// <summary>
        /// Represents the value at level 0
        /// </summary>
        float BaseValue { get; }
        /// <summary>
        /// Represents the value that you add for each level
        /// </summary>
        float ValuePerLevel { get; }
        /// <summary>
        /// Represents the cost at level 0
        /// </summary>
        int BaseCost { get; }
        /// <summary>
        /// Represents the cost that you add for each level
        /// </summary>
        int CostPerLevel { get; }
        /// <summary>
        /// Represents the maximum level the perk can reach
        /// </summary>
        int MaxLevel { get; }
        /// <summary>
        /// Represents the current perk level
        /// </summary>
        int Level { get; }
        /// <summary>
        /// Represents the value of the current level
        /// </summary>
        float Value { get; }
        /// <summary>
        /// Represents the cost of the current level
        /// </summary>
        int Cost { get; }

        /// <summary>
        /// Used to setup the perk, it must be called before using the perk
        /// You can give it a IDataManager to store the Value inside with the PerkName as key
        /// It will only be in RAM without an IDataManager, in disk with IDataManager
        /// </summary>
        void Init(IDataManager dataManager = null);
        /// <summary>
        /// Used to overwrite the perk's value to the value given in parameter
        /// </summary>
        /// <param name="level">New level</param>
        void SetLevel(int level);
        /// <summary>
        /// Used to increase the level by 1 if possible
        /// </summary>
        void LevelUp();
        /// <summary>
        /// Used to reset the perk at level 0
        /// </summary>
        void Reset();
        /// <summary>
        /// Used to determine whether or not the perk has reached max level
        /// </summary>
        bool IsMaxLevel();
    }
}