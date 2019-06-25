/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace APIS2.Models
{
    using System;
    using System.Linq;

    public static class User
    {
        public static readonly string TYPE_MANAGER = "MANAGER";
        public static readonly string TYPE_OPERATOR = "OPERATOR";

        public static bool ValidateUser(string myUsername, string myPassword, out string type)
        {
            bool validated = false;
            try
            {
                MicroTechDatabaseContext database = new MicroTechDatabaseContext();
                type = string.Empty;

                var users = database.Employees.ToList();

                for (int i = 0; i < users.Count; i++)
                {
                    if (myUsername.Equals(users.ElementAt(i).NickName) &&
                        myPassword.Equals(users.ElementAt(i).Password))
                    {
                        validated = true;
                        type = users.ElementAt(i).Type;
                    }
                }

                database.Dispose();
            }
            catch (Exception ex)
            {
                string temp = ex.Message;
                string toString = ex.ToString();
                type = string.Empty;
                validated = false;
            }

            return validated;
        }

        /// <summary>
        /// Fills the database.
        /// Used just once in the beggining.
        /// </summary>
        public static void FillDatabase()
        {
            MicroTechDatabaseContext database = new MicroTechDatabaseContext();

            database.Employees.Add(new Employee() {ID=1,FirstName="Ladislav", LastName="Pomsar", NickName="laci",Password="123456",Type=Models.User.TYPE_OPERATOR});
            database.Employees.Add(new Employee() {ID=2,FirstName="Tomas", LastName="Goffa", NickName="tom", Password="123456", Type=Models.User.TYPE_MANAGER });

            int OutputResourceID = 1;

            string processorType = CustomerPage.PROC_AT89C;
            string[] flash = new string[] { CustomerPage.FLASH_2KB, CustomerPage.FLASH_4KB };
            string[] ram = new string[] { CustomerPage.RAM_128B, CustomerPage.RAM_256B, CustomerPage.RAM_512B };
            string[] eeprom = new string[] { CustomerPage.EEPROM_NONE, CustomerPage.EEPROM_2KB };
            string[] freq = new string[] { CustomerPage.FREQ_8MHZ, CustomerPage.FREQ_16MHZ, CustomerPage.FREQ_24MHZ, CustomerPage.FREQ_32MHZ };

            for (int i = 0; i < flash.Length; i++)
            {
                for (int j = 0; j < ram.Length; j++)
                {
                    for (int k = 0; k < eeprom.Length; k++)
                    {
                        for (int m = 0; m < freq.Length; m++)
                        {
                            database.OutputResources.Add(new OutputResource
                            {
                                ID = OutputResourceID,
                                ProcessorType = processorType,
                                Flash = flash[i],
                                RAM = ram[j],
                                EEPROM = eeprom[k],
                                Frequency = freq[m],
                                NumberOfChips = 0
                            });

                            OutputResourceID++;
                        }
                    }
                }
            }

            processorType = CustomerPage.PROC_ATTINY;
            flash = new string[] { CustomerPage.FLASH_2KB, CustomerPage.FLASH_4KB };
            ram = new string[] { CustomerPage.RAM_128B, CustomerPage.RAM_256B, CustomerPage.RAM_512B };
            eeprom = new string[] { CustomerPage.EEPROM_NONE, CustomerPage.EEPROM_512B };

            for (int i = 0; i < flash.Length; i++)
            {
                for (int j = 0; j < ram.Length; j++)
                {
                    for (int k = 0; k < eeprom.Length; k++)
                    {
                        for (int m = 0; m < freq.Length; m++)
                        {
                            database.OutputResources.Add(new OutputResource
                            {
                                ID = OutputResourceID,
                                ProcessorType = processorType,
                                Flash = flash[i],
                                RAM = ram[j],
                                EEPROM = eeprom[k],
                                Frequency = freq[m],
                                NumberOfChips = 0
                            });

                            OutputResourceID++;
                        }
                    }
                }
            }

            processorType = CustomerPage.PROC_PIC;
            flash = new string[] { CustomerPage.FLASH_1_5KB, CustomerPage.FLASH_3_5KB };
            ram = new string[] { CustomerPage.RAM_512B, CustomerPage.RAM_1KB };
            eeprom = new string[] { CustomerPage.EEPROM_NONE, CustomerPage.EEPROM_2KB };

            for (int i = 0; i < flash.Length; i++)
            {
                for (int j = 0; j < ram.Length; j++)
                {
                    for (int k = 0; k < eeprom.Length; k++)
                    {
                        for (int m = 0; m < freq.Length; m++)
                        {
                            database.OutputResources.Add(new OutputResource
                            {
                                ID = OutputResourceID,
                                ProcessorType = processorType,
                                Flash = flash[i],
                                RAM = ram[j],
                                EEPROM = eeprom[k],
                                Frequency = freq[m],
                                NumberOfChips = 0
                            });

                            OutputResourceID++;
                        }
                    }
                }
            }

            database.InputResources.Add(new InputResource()
            {
                ID = 1,
                Name = "Sand",
                Count = 25000
            });

            database.InputResources.Add(new InputResource()
            {
                ID = 1,
                Name = "Copper",
                Count = 1000
            });

            database.SaveChanges();
        }
    }
}