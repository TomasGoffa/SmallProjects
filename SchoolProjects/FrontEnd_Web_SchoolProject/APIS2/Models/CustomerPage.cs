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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class CustomerPage
    {
        [Required]
        [Display(Name = "Processor Type")]
        public string ProcessorType { get; set; }

        [Required]
        [Display(Name = "Flash")]
        public string FlashMemory { get; set; }

        [Required]
        [Display(Name = "RAM")]
        public string RamMemory { get; set; }

        [Required]
        [Display(Name = "EEPROM")]
        public string Eeprom { get; set; }

        [Required]
        [Display(Name = "Frequency")]
        public string Frequency { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        [Display(Name = "Number of micro processors")]
        public int NumberOfChips { get; set; }

        public List<SelectListItem> PossibleProcesssorTypes { get; set; }
        public List<SelectListItem> PossibleFlashMemory { get; set; }
        public List<SelectListItem> PossibleRamMemory { get; set; }
        public List<SelectListItem> PossibleEeprom { get; set; }
        public List<SelectListItem> PossibleFrequency { get; set; }

        public CustomerPage()
        {
            this.PossibleProcesssorTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = PROC_AT89C, Value = PROC_AT89C, Selected = true },
                new SelectListItem { Text = PROC_ATTINY, Value = PROC_ATTINY },
                new SelectListItem { Text = PROC_PIC, Value = PROC_PIC }
            };

            this.PossibleFlashMemory = new List<SelectListItem>
            {
                new SelectListItem { Text = FLASH_1_5KB, Value = FLASH_1_5KB },
                new SelectListItem { Text = FLASH_2KB, Value = FLASH_2KB },
                new SelectListItem { Text = FLASH_3_5KB, Value = FLASH_3_5KB },
                new SelectListItem { Text = FLASH_4KB, Value = FLASH_4KB }
            };

            this.PossibleRamMemory = new List<SelectListItem>
            {
                new SelectListItem { Text = RAM_128B, Value = RAM_128B },
                new SelectListItem { Text = RAM_256B, Value = RAM_256B },
                new SelectListItem { Text = RAM_512B, Value = RAM_512B },
                new SelectListItem { Text = RAM_1KB, Value = RAM_1KB }
            };

            this.PossibleEeprom = new List<SelectListItem>
            {
                new SelectListItem { Text = EEPROM_NONE, Value = EEPROM_NONE },
                new SelectListItem { Text = EEPROM_512B, Value = EEPROM_512B },
                new SelectListItem { Text = EEPROM_2KB, Value = EEPROM_2KB }
            };

            this.PossibleFrequency = new List<SelectListItem>
            {
                new SelectListItem { Text = FREQ_8MHZ, Value = FREQ_8MHZ },
                new SelectListItem { Text = FREQ_16MHZ, Value = FREQ_16MHZ },
                new SelectListItem { Text = FREQ_24MHZ, Value = FREQ_24MHZ },
                new SelectListItem { Text = FREQ_32MHZ, Value = FREQ_32MHZ }
            };

            this.NumberOfChips = 1;
        }
        #region Constants
        public static readonly string PROC_AT89C = "AT 89C";
        public static readonly string PROC_ATTINY = "ATTINY";
        public static readonly string PROC_PIC = "PIC";

        public static readonly string FLASH_1_5KB = "1,5 kB";
        public static readonly string FLASH_2KB = "2 kB";
        public static readonly string FLASH_3_5KB = "3,5 kB";
        public static readonly string FLASH_4KB = "4 kB";

        public static readonly string RAM_128B = "128 B";
        public static readonly string RAM_256B = "256 B";
        public static readonly string RAM_512B = "512 B";
        public static readonly string RAM_1KB = "1 kB";

        public static readonly string EEPROM_NONE = "None";
        public static readonly string EEPROM_512B = "512 B";
        public static readonly string EEPROM_2KB = "2 kB";

        public static readonly string FREQ_8MHZ = "8 Mhz";
        public static readonly string FREQ_16MHZ = "16 Mhz";
        public static readonly string FREQ_24MHZ = "24 Mhz";
        public static readonly string FREQ_32MHZ = "32 Mhz";
        #endregion Constants
    }
}