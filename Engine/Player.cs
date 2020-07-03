using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public Location CurrentLocation { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        
        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int level) :
                    base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if(location.ItemRequiredToEnter == null)
            {
                //There is no required item

                return true;
            }

            //See if the player has required item
            foreach (InventoryItem ii in Inventory)
            {
                if(ii.Details.ID == location.ItemRequiredToEnter.ID)
                {
                    //Item found
                    return true;
                }
            }

            //Required item not found
            return false;
        }

        public bool HasThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                bool foundItemInPlayersInventory = false;

                //Check each item in inventory to see if they have it and enought of it
                foreach (InventoryItem ii in Inventory)
                {
                    if(ii.Details.ID == qci.Details.ID)
                    {
                        foundItemInPlayersInventory = true;
                        if(ii.Quantity < qci.Quantity)
                        {
                            //there is not enough of this item to complete quest
                            return false;
                        }
                    }
                }

                if(!foundItemInPlayersInventory)
                {
                    //Player does not have competion item in inventory
                    return false;
                }
            }

            //if we got here then player must have all the required items to complete the quest
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                foreach (InventoryItem ii in Inventory)
                {
                    if(ii.Details.ID == qci.Details.ID)
                    {
                        //Substract the quantity from player's inventory
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach (InventoryItem ii in Inventory)
            {
                if(ii.Details.ID == itemToAdd.ID)
                {
                    //The player has the item in inventory, so increase by 1
                    ii.Quantity++;
                    return;
                }
            }

            //The item was not in inventory
            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }

        public void MarkQuestCompleted(Quest quest)
        {
            //Find quest in player quest list
            foreach (PlayerQuest pq in Quests)
            {
                if(pq.Details.ID == quest.ID)
                {
                    pq.IsCompleted = true;
                    return;
                }
            }
        }
    }
}
