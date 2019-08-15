﻿namespace ShibaBot.Models {
    public  class LocalesModel {
        public ErrorsModel Errors;
        public string Mention;
        public ModulesModel Modules;

        public class ErrorsModel {
            public string BadArgCount { set; get; }
            public string ObjectNotFound { set; get; }
            public string UnknownCommand { set; get; }
            public UnmetConditionModel UnmetCondition { set; get; }
        }

        public class UnmetConditionModel {
            public string GuildOnly;
        }
        public class ModulesModel {
            public ImageModel Image { set; get; }
        }
        public class ImageModel {
            public string Shibe { set; get; }
        }

    }
}