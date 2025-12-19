using Microsoft.ML;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Training
{
    public class TrainSentiment
    {
        public void Train()
        {
            var ml = new MLContext(seed: 42);

            // CSV yolu (ekrandaki gibi)
            // CSV header: MessageSubject,MessageText,SentimentLabel
            var dataPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "data", "100_messages.csv");

            var data = ml.Data.LoadFromTextFile<MessageModelInput>(
                path: dataPath,
                hasHeader: true,
                separatorChar: ','
            );
            var split = ml.Data.TrainTestSplit(data, 0.2);

            // Ekrandaki pipeline mantığı:
            // 1) MessageText -> TextFeats
            // 2) MessageSubject -> TopicFeats
            // 3) Features = TextFeats + TopicFeats
            // 4) SentimentLabel -> Label (Key)
            // 5) Trainer: SdcaMaximumEntropy (Multiclass)
            // 6) PredictedLabel geri stringe dönsün
            var pipeline = ml.Transforms.Text.FeaturizeText("TextFeats", nameof(MessageModelInput.MessageText))
                .Append(ml.Transforms.Text.FeaturizeText("TopicFeats", nameof(MessageModelInput.MessageSubject)))
                .Append(ml.Transforms.Concatenate("Features", "TextFeats", "TopicFeats"))
                .Append(ml.Transforms.Conversion.MapValueToKey("Label", nameof(MessageModelInput.SentimentLabel)))
                .Append(ml.MulticlassClassification.Trainers.SdcaMaximumEntropy())
                .Append(ml.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var model = pipeline.Fit(split.TrainSet);

            // ---- (opsiyonel ama öneririm) değerlendirme ----
            var preds = model.Transform(split.TestSet);

            var metrics = ml.MulticlassClassification.Evaluate(
                data: preds,
                labelColumnName: "Label",
                scoreColumnName: "Score"
            );

          

            // ---- modeli kaydet (zip) ----
            var modelPath = Path.Combine(AppContext.BaseDirectory, "MLModels", "sentiment_model.zip");
            Directory.CreateDirectory(Path.GetDirectoryName(modelPath)!);

            ml.Model.Save(model, split.TrainSet.Schema, modelPath);

          
        }
    }
}
