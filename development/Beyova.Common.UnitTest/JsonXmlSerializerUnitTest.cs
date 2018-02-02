//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace Beyova.Common.UnitTest
//{
//    [TestClass]
//    public class JsonXmlSerializerUnitTest
//    {
//        [TestMethod]
//        public void Test()
//        {
//            Dictionary<string, object> item = new Dictionary<string, object>();
//            item.Add("key1", 1);
//            item.Add("key2", 1.1);
//            item.Add("key3", true);
//            item.Add("key4", "string1");
//            item.Add("key5", new[] { "string1", "string2" });

//            item.Add("key6", Guid.NewGuid());
//            item.Add("key7", DateTime.Now);
//            item.Add("key8", new TimeSpan(10030));

//            item.Add("key9", Encoding.UTF8.GetBytes("Hello World"));

//            item.Add("1", 1);

//            JToken token = JToken.Parse(JsonConvert.SerializeObject(item));

//            var y = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(item));

//            Assert.IsNotNull(y);

//            var xml2 = JsonXmlizer.Xmlize(token);
//            Assert.IsNotNull(xml2);

//            var dic2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(xml2.Dexmlize().ToString());
//            Assert.IsNotNull(dic2);

//            Assert.AreEqual(Convert.ToInt32(dic2["1"]), 1);

//            return;
//        }

//        [TestMethod]
//        public void Complex()
//        {
//            var jtoken = JToken.Parse(@"{
//  ""submitRequest"": {
//                ""CourseKey"": ""c79885fe-9fc8-4823-b19c-e93eaef0a837"",
//    ""StudentId"": ""12204771"",
//    ""Answers"": [
//      {
//        ""ActivityKey"": ""121049c8-3420-4db5-8b38-dc55f78e0674"",
//        ""ActivitySnapshotKey"": ""77fe526c-38ba-427f-b818-a35004d2c88a"",
//        ""CorrectAnswers"": [
//          {
//            ""options"": [
//              ""weather is hot"",
//              ""weather is cold"",
//              ""weather is cool""
//            ]
//    },
//          {
//            ""options"": [
//              ""west china"",
//              ""north china"",
//              ""south china"",
//              ""north east china"",
//              ""east china""
//            ]
//}
//        ],
//        ""StudentAnswers"": {
//          ""simpleAnswers"": [
//            {
//              ""options"": [
//                ""weather is hot"",
//                ""weather is cold"",
//                ""weather is cool""
//              ]
//            },
//            {
//              ""options"": [
//                ""south china"",
//                ""east china"",
//                ""west china"",
//                ""north east china"",
//                ""north china""
//              ]
//            }
//          ],
//          ""modelData"": {
//            ""currentPageIndex"": 1,
//            ""activityId"": ""tb16/book-3/unit-5/activities/homework/matchimageaudiototext-texttoimagex"",
//            ""templateId"": ""matchImageAudioToText"",
//            ""customized"": true,
//            ""pages"": [
//              {
//                ""config"": {
//                  ""heightDrop"": 84,
//                  ""fontSizeDrop"": 14,
//                  ""fontSizeDrag"": 18,
//                  ""widthDrag"": 304,
//                  ""topDrop"": 273,
//                  ""marginDrag"": 10,
//                  ""marginDrop"": 10,
//                  ""scaleRatio"": 0.04,
//                  ""widthDrop"": 192,
//                  ""heightDrag"": 130
//                },
//                ""transition"": ""none"",
//                ""_-_-hashKey"": ""object:34"",
//                ""dragList"": [
//                  {
//                    ""index"": 1,
//                    ""tdIndex"": 1,
//                    ""text"": ""weather is cold""
//                  },
//                  {
//                    ""index"": 0,
//                    ""tdIndex"": 0,
//                    ""text"": ""weather is hot""
//                  },
//                  {
//                    ""index"": 2,
//                    ""tdIndex"": 2,
//                    ""text"": ""weather is cool""
//                  }
//                ],
//                ""isCorrect"": true,
//                ""title"": {
//                  ""texts"": [
//                    {
//                      ""text"": ""frdf""
//                    }
//                  ]
//                },
//                ""numTried"": 1,
//                ""dropList"": [
//                  {
//                    ""answer"": ""weather is hot"",
//                    ""status"": ""correctStrong"",
//                    ""mp3Url"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""droppedItemIndex"": 0,
//                    ""ogaUrl"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""_-_-hashKey"": ""object:40"",
//                    ""image"": ""../com.ef.e1.resouce.manager/binary/3F5FCB07-0D25-44BB-9E1B-2A66B345DC57.jpeg"",
//                    ""text"": """",
//                    ""audioPlayTimes"": 0,
//                    ""audioTotalPlayTimes"": 2,
//                    ""index"": 0
//                  },
//                  {
//                    ""answer"": ""weather is cold"",
//                    ""status"": ""correctStrong"",
//                    ""mp3Url"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""droppedItemIndex"": 1,
//                    ""ogaUrl"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""_-_-hashKey"": ""object:41"",
//                    ""image"": ""../com.ef.e1.resouce.manager/binary/0CFA1AF0-23F5-4200-8109-B7EE149609DC.jpeg"",
//                    ""text"": """",
//                    ""audioPlayTimes"": 0,
//                    ""audioTotalPlayTimes"": 2,
//                    ""index"": 1
//                  },
//                  {
//                    ""answer"": ""weather is cool"",
//                    ""status"": ""correctStrong"",
//                    ""mp3Url"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""droppedItemIndex"": 2,
//                    ""ogaUrl"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""_-_-hashKey"": ""object:42"",
//                    ""image"": ""../com.ef.e1.resouce.manager/binary/6799204F-D98A-449F-8B02-C748B64B9FEF.jpeg"",
//                    ""text"": """",
//                    ""audioPlayTimes"": 0,
//                    ""audioTotalPlayTimes"": 2,
//                    ""index"": 2
//                  }
//                ],
//                ""numAnswers"": 3,
//                ""index"": 0
//              },
//              {
//                ""config"": {
//                  ""heightDrop"": 84,
//                  ""fontSizeDrop"": 14,
//                  ""fontSizeDrag"": 18,
//                  ""widthDrag"": 304,
//                  ""topDrop"": 273,
//                  ""marginDrag"": 10,
//                  ""marginDrop"": 10,
//                  ""scaleRatio"": 0.04,
//                  ""widthDrop"": 192,
//                  ""heightDrag"": 82
//                },
//                ""transition"": ""none"",
//                ""_-_-hashKey"": ""object:35"",
//                ""dragList"": [
//                  {
//                    ""index"": 2,
//                    ""tdIndex"": 0,
//                    ""text"": ""south china""
//                  },
//                  {
//                    ""index"": 4,
//                    ""tdIndex"": 1,
//                    ""text"": ""east china""
//                  },
//                  {
//                    ""index"": 5,
//                    ""text"": ""guess whether i am a distractor or not"",
//                    ""tdIndex"": -1
//                  },
//                  {
//                    ""index"": 0,
//                    ""tdIndex"": 2,
//                    ""text"": ""west china""
//                  },
//                  {
//                    ""index"": 3,
//                    ""tdIndex"": 3,
//                    ""text"": ""north east china""
//                  },
//                  {
//                    ""index"": 1,
//                    ""tdIndex"": 4,
//                    ""text"": ""north china""
//                  }
//                ],
//                ""isCorrect"": false,
//                ""title"": {
//                  ""texts"": [
//                    {
//                      ""text"": ""kjk fdffg""
//                    }
//                  ]
//                },
//                ""numTried"": 2,
//                ""dropList"": [
//                  {
//                    ""answer"": ""west china"",
//                    ""status"": ""incorrectStrong"",
//                    ""mp3Url"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""droppedItemIndex"": 2,
//                    ""ogaUrl"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""_-_-hashKey"": ""object:61"",
//                    ""image"": ""../com.ef.e1.resouce.manager/binary/5B96C73D-CE05-4E58-B464-7818EC1DF491.jpeg"",
//                    ""text"": """",
//                    ""audioPlayTimes"": 0,
//                    ""audioTotalPlayTimes"": 2,
//                    ""index"": 0
//                  },
//                  {
//                    ""answer"": ""north china"",
//                    ""status"": ""incorrectStrong"",
//                    ""mp3Url"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""droppedItemIndex"": 4,
//                    ""ogaUrl"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""_-_-hashKey"": ""object:62"",
//                    ""image"": ""../com.ef.e1.resouce.manager/binary/DF439025-27C5-40AE-9238-FE85538BFC87.jpeg"",
//                    ""text"": """",
//                    ""audioPlayTimes"": 0,
//                    ""audioTotalPlayTimes"": 2,
//                    ""index"": 1
//                  },
//                  {
//                    ""answer"": ""south china"",
//                    ""status"": ""incorrectStrong"",
//                    ""mp3Url"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""droppedItemIndex"": 0,
//                    ""ogaUrl"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""_-_-hashKey"": ""object:63"",
//                    ""image"": ""../com.ef.e1.resouce.manager/binary/564513C2-E899-41A5-BCCC-290343D32EC5.jpeg"",
//                    ""text"": """",
//                    ""audioPlayTimes"": 0,
//                    ""audioTotalPlayTimes"": 2,
//                    ""index"": 2
//                  },
//                  {
//                    ""answer"": ""north east china"",
//                    ""status"": ""correctSoft"",
//                    ""mp3Url"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""droppedItemIndex"": 3,
//                    ""ogaUrl"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""_-_-hashKey"": ""object:64"",
//                    ""image"": ""../com.ef.e1.resouce.manager/binary/1A2C197F-BF93-4355-952A-B8794BCAC004.jpeg"",
//                    ""text"": """",
//                    ""audioPlayTimes"": 0,
//                    ""audioTotalPlayTimes"": 2,
//                    ""index"": 3
//                  },
//                  {
//                    ""answer"": ""east china"",
//                    ""status"": ""incorrectStrong"",
//                    ""mp3Url"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""droppedItemIndex"": 1,
//                    ""ogaUrl"": ""../com.ef.e1.resouce.manager/binary/E3E28617-8897-4E9E-8E05-27F0C97BBF14.mp3"",
//                    ""_-_-hashKey"": ""object:65"",
//                    ""image"": ""../com.ef.e1.resouce.manager/binary/A583D28B-ABFE-4ABF-B57A-01944D040A2F.jpeg"",
//                    ""text"": """",
//                    ""audioPlayTimes"": 0,
//                    ""audioTotalPlayTimes"": 2,
//                    ""index"": 4
//                  }
//                ],
//                ""numAnswers"": 5,
//                ""index"": 1
//              }
//            ],
//            ""key"": ""121049c8-3420-4db5-8b38-dc55f78e0674"",
//            ""snapshotKey"": ""77fe526c-38ba-427f-b818-a35004d2c88a"",
//            ""skillType"": ""Grammar"",
//            ""instruction"": ""Landscapes of china.""
//          }
//        },
//        ""Effort"": null,
//        ""StudentScore"": 4.0,
//        ""TotalScore"": 8.0,
//        ""TotalQuestionCount"": 2,
//        ""CompleteQuestionCount"": 2,
//        ""CorrectQuestionCount"": 1,
//        ""AnswerType"": 0
//      },
//      {
//        ""ActivityKey"": ""2c625613-75d0-4b9b-9074-60a8b3affb99"",
//        ""ActivitySnapshotKey"": ""4c3d68ab-05e2-46a5-b5c5-565b5a12a410"",
//        ""CorrectAnswers"": [
//          {
//            ""options"": [
//              ""about one's dream"",
//              ""about one's life""
//            ]
//          }
//        ],
//        ""StudentAnswers"": {
//          ""simpleAnswers"": [
//            {
//              ""options"": [
//                ""about one's life"",
//                ""about one's friend""
//              ]
//            }
//          ],
//          ""modelData"": {
//            ""currentPageIndex"": 0,
//            ""activityId"": ""tb16/book-3/unit-5/activities/homework/multipleselectlongtext-multipleselect"",
//            ""templateId"": ""multipleSelectLongText"",
//            ""customized"": true,
//            ""originalStimulus"": ""<p>Everyone has their own dreams, I am the same. But my dream is not a lawyer, not a doctor, not actors, not even an industry. Perhaps my dream big people will find it ridiculous, but this has been my pursuit! My dream is to want to have a folk life! I want it to become a beautiful painting, it is not only sharp colors, but also the colors are bleak, I do not rule out the painting is part of the black, but I will treasure these bleak colors! Not yet, how about, a colorful painting, if not bleak, add color, how can it more prominent American? Life is like painting, painting the bright red color represents life beautiful happy moments. Painting a bleak color represents life difficult, unpleasant time. You may find a flat with a beautiful road is not very good yet, but I do not think it will. If a person lives flat then what is the point? Life is only a short few decades, I want it to go Finally, Each memory is a solid.</p>"",
//            ""pages"": [
//              {
//                ""_-_-hashKey"": ""object:15"",
//                ""selectedOptionIndexs"": [],
//                ""transition"": ""none"",
//                ""stimulus"": """",
//                ""numAnswers"": 2,
//                ""numTried"": 2,
//                ""question"": ""what's the meaning of this article?"",
//                ""numSelected"": 0,
//                ""completed"": true,
//                ""options"": [
//                  {
//                    ""text"": ""about one's dream"",
//                    ""status"": ""correctStrong"",
//                    ""_-_-hashKey"": ""object:20"",
//                    ""isAnswer"": true,
//                    ""index"": 4
//                  },
//                  {
//                    ""text"": ""about one's life"",
//                    ""status"": ""correctSoft"",
//                    ""_-_-hashKey"": ""object:21"",
//                    ""isAnswer"": true,
//                    ""index"": 1
//                  },
//                  {
//                    ""text"": ""about one's work"",
//                    ""status"": ""normal"",
//                    ""_-_-hashKey"": ""object:22"",
//                    ""isAnswer"": false,
//                    ""index"": 0
//                  },
//                  {
//                    ""text"": ""about one's friend"",
//                    ""status"": ""incorrectSoft"",
//                    ""_-_-hashKey"": ""object:23"",
//                    ""isAnswer"": false,
//                    ""index"": 2
//                  },
//                  {
//                    ""text"": ""about one's life style"",
//                    ""status"": ""normal"",
//                    ""_-_-hashKey"": ""object:24"",
//                    ""isAnswer"": false,
//                    ""index"": 3
//                  }
//                ]
//              }
//            ],
//            ""stimulus"": {},
//            ""key"": ""2c625613-75d0-4b9b-9074-60a8b3affb99"",
//            ""snapshotKey"": ""4c3d68ab-05e2-46a5-b5c5-565b5a12a410"",
//            ""skillType"": ""Listening"",
//            ""instruction"": ""read the long text and select the right answer.""
//          }
//        },
//        ""Effort"": null,
//        ""StudentScore"": 1.0,
//        ""TotalScore"": 2.0,
//        ""TotalQuestionCount"": 1,
//        ""CompleteQuestionCount"": 1,
//        ""CorrectQuestionCount"": 0,
//        ""AnswerType"": 0
//      },
//      {
//        ""ActivityKey"": ""cb3a910a-4253-46ff-b5b3-c084da611aef"",
//        ""ActivitySnapshotKey"": ""b68baee3-02e1-475e-a20c-b15c3a326002"",
//        ""CorrectAnswers"": [
//          {
//            ""gaps"": [
//              ""friend"",
//              ""person-Updated"",
//              ""warm"",
//              ""depressed"",
//              ""best"",
//              ""favorite"",
//              ""good"",
//              ""each other"",
//              ""forever and ever""
//            ]
//          }
//        ],
//        ""StudentAnswers"": {
//          ""simpleAnswers"": [
//            {
//              ""gaps"": [
//                ""forever and ever"",
//                ""favorite"",
//                ""warm"",
//                ""each other"",
//                ""depressed"",
//                ""person-Updated"",
//                ""friend"",
//                ""Add Distractors"",
//                ""best""
//              ]
//            }
//          ],
//          ""modelData"": {
//            ""currentPageIndex"": 0,
//            ""activityId"": ""tb16/book-3/unit-5/activities/homework/gapfillimageandlongtext-gapfill20160128"",
//            ""templateId"": ""gapFillImageAndLongText"",
//            ""customized"": true,
//            ""pages"": [
//              {
//                ""answerList"": [
//                  {
//                    ""index"": 1,
//                    ""dropIndex"": 8,
//                    ""text"": ""forever and ever""
//                  },
//                  {
//                    ""index"": 3,
//                    ""dropIndex"": 5,
//                    ""text"": ""favorite""
//                  },
//                  {
//                    ""index"": 5,
//                    ""dropIndex"": 2,
//                    ""text"": ""warm""
//                  },
//                  {
//                    ""index"": 7,
//                    ""dropIndex"": 7,
//                    ""text"": ""each other""
//                  },
//                  {
//                    ""index"": 9,
//                    ""dropIndex"": 3,
//                    ""text"": ""depressed""
//                  },
//                  {
//                    ""index"": 11,
//                    ""dropIndex"": 1,
//                    ""text"": ""person-Updated""
//                  },
//                  {
//                    ""index"": 13,
//                    ""dropIndex"": 0,
//                    ""text"": ""friend""
//                  },
//                  {
//                    ""index"": 15,
//                    ""dropIndex"": 10,
//                    ""text"": ""Add Distractors""
//                  },
//                  {
//                    ""index"": 17,
//                    ""dropIndex"": 4,
//                    ""text"": ""best""
//                  }
//                ],
//                ""correctAnswerList"": [
//                  ""friend"",
//                  ""person-Updated"",
//                  ""warm"",
//                  ""depressed"",
//                  ""best"",
//                  ""favorite"",
//                  ""good"",
//                  ""each other"",
//                  ""forever and ever""
//                ],
//                ""pageMaxInputSize"": 195,
//                ""pageIndex"": 0,
//                ""stimulus"": ""../com.ef.e1.resouce.manager/binary/A12B3B22-8446-44F3-BAE3-D06364964A09.png"",
//                ""completed"": true,
//                ""isAllCorrect"": false,
//                ""numTried"": 2,
//                ""question"": {
//                  ""rawContent"": ""<div class='ar-gap-dd-image-area'><img ar-gap-dd-image-wrapper=\""\"" /></div>A <span ar-gap-dd-value=\""friend\"">friend</span> is a <span ar-gap-dd-value=\""person-Updated\"">person-Updated</span> who can let you feel <span ar-gap-dd-value=\""warm\"">warm</span> when you are <span ar-gap-dd-value=\""depressed\"">depressed</span>. I have many friends.But Fisher is my <span ar-gap-dd-value=\""best\"">best</span> friend.He is as old as me.He is taller than me.Basketball is his <span ar-gap-dd-value=\""favorite\"">favorite</span> sport.We are in the same class.He is good at study.So his study is very <span ar-gap-dd-value=\""good\"">good</span>.We learn from each other and help <span ar-gap-dd-value=\""each other\"">each other</span>.He will help me if i got in trouble.I will help he as much as I can. I hope our friendship will <span ar-gap-dd-value=\""forever and ever\"">forever and ever</span>. A friend is someone who can let you feel warm when you are depressed. I have many friends. However I only have one best friend and it's Fisher. He is the same age as me. He is <span ar-gap-dd-value=\""taller\"" data-gap-example=\""true\"">taller</span> than me. Basketball is his favourite sport. We are in the same class. He likes studying and is good at it, so he gets high marks. We learn from each other and help each other. He will help me if I ever get into troubles. So will I. I hope our friendship will last forever and ever."",
//                  ""htmlContent"": {},
//                  ""title"": ""who is your best friend?-Update{\""123\""}"",
//                  ""finalArr"": [
//                    {
//                      ""isTyping"": false,
//                      ""val"": ""<div class='ar-gap-dd-image-area'><img ar-gap-dd-image-wrapper=\""\"" /></div>A "",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    },
//                    {
//                      ""status"": ""outOfChances"",
//                      ""isExample"": false,
//                      ""val"": ""<span ar-gap-dd-value=\""friend\"">friend</span>"",
//                      ""html"": {},
//                      ""isTyping"": false,
//                      ""inputValue"": {
//                        ""text"": ""forever and ever"",
//                        ""filledIndex"": 1,
//                        ""performance"": """",
//                        ""belong"": ""dropArea"",
//                        ""index"": 8
//                      },
//                      ""correctAns"": ""friend"",
//                      ""isGap"": true,
//                      ""incorrectIndex"": 0
//                    },
//                    {
//                      ""isTyping"": false,
//                      ""val"": "" is a "",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    },
//                    {
//                      ""status"": ""outOfChances"",
//                      ""isExample"": false,
//                      ""val"": ""<span ar-gap-dd-value=\""person-Updated\"">person-Updated</span>"",
//                      ""html"": {},
//                      ""isTyping"": false,
//                      ""inputValue"": {
//                        ""text"": ""favorite"",
//                        ""filledIndex"": 3,
//                        ""performance"": """",
//                        ""belong"": ""dropArea"",
//                        ""index"": 5
//                      },
//                      ""correctAns"": ""person-Updated"",
//                      ""isGap"": true,
//                      ""incorrectIndex"": 1
//                    },
//                    {
//                      ""isTyping"": false,
//                      ""val"": "" who can let you feel "",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    },
//                    {
//                      ""status"": ""correctSoft"",
//                      ""isExample"": false,
//                      ""val"": ""<span ar-gap-dd-value=\""warm\"">warm</span>"",
//                      ""html"": {},
//                      ""isTyping"": false,
//                      ""inputValue"": {
//                        ""text"": ""warm"",
//                        ""filledIndex"": 5,
//                        ""performance"": ""none"",
//                        ""belong"": ""dropArea"",
//                        ""index"": 2
//                      },
//                      ""correctAns"": ""warm"",
//                      ""isGap"": true
//                    },
//                    {
//                      ""isTyping"": false,
//                      ""val"": "" when you are "",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    },
//                    {
//                      ""status"": ""outOfChances"",
//                      ""isExample"": false,
//                      ""val"": ""<span ar-gap-dd-value=\""depressed\"">depressed</span>"",
//                      ""html"": {},
//                      ""isTyping"": false,
//                      ""inputValue"": {
//                        ""text"": ""each other"",
//                        ""filledIndex"": 7,
//                        ""performance"": """",
//                        ""belong"": ""dropArea"",
//                        ""index"": 7
//                      },
//                      ""correctAns"": ""depressed"",
//                      ""isGap"": true,
//                      ""incorrectIndex"": 2
//                    },
//                    {
//                      ""isTyping"": false,
//                      ""val"": "". I have many friends.But Fisher is my "",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    },
//                    {
//                      ""status"": ""outOfChances"",
//                      ""isExample"": false,
//                      ""val"": ""<span ar-gap-dd-value=\""best\"">best</span>"",
//                      ""html"": {},
//                      ""isTyping"": false,
//                      ""inputValue"": {
//                        ""text"": ""depressed"",
//                        ""filledIndex"": 9,
//                        ""performance"": """",
//                        ""belong"": ""dropArea"",
//                        ""index"": 3
//                      },
//                      ""correctAns"": ""best"",
//                      ""isGap"": true,
//                      ""incorrectIndex"": 3
//                    },
//                    {
//                      ""isTyping"": false,
//                      ""val"": "" friend.He is as old as me.He is taller than me.Basketball is his "",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    },
//                    {
//                      ""status"": ""outOfChances"",
//                      ""isExample"": false,
//                      ""val"": ""<span ar-gap-dd-value=\""favorite\"">favorite</span>"",
//                      ""html"": {},
//                      ""isTyping"": false,
//                      ""inputValue"": {
//                        ""text"": ""person-Updated"",
//                        ""filledIndex"": 11,
//                        ""performance"": """",
//                        ""belong"": ""dropArea"",
//                        ""index"": 1
//                      },
//                      ""correctAns"": ""favorite"",
//                      ""isGap"": true,
//                      ""incorrectIndex"": 4
//                    },
//                    {
//                      ""isTyping"": false,
//                      ""val"": "" sport.We are in the same class.He is good at study.So his study is very "",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    },
//                    {
//                      ""status"": ""outOfChances"",
//                      ""isExample"": false,
//                      ""val"": ""<span ar-gap-dd-value=\""good\"">good</span>"",
//                      ""html"": {},
//                      ""isTyping"": false,
//                      ""inputValue"": {
//                        ""text"": ""friend"",
//                        ""filledIndex"": 13,
//                        ""performance"": """",
//                        ""belong"": ""dropArea"",
//                        ""index"": 0
//                      },
//                      ""correctAns"": ""good"",
//                      ""isGap"": true,
//                      ""incorrectIndex"": 5
//                    },
//                    {
//                      ""isTyping"": false,
//                      ""val"": "".We learn from each other and help "",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    },
//                    {
//                      ""status"": ""outOfChances"",
//                      ""isExample"": false,
//                      ""val"": ""<span ar-gap-dd-value=\""each other\"">each other</span>"",
//                      ""html"": {},
//                      ""isTyping"": false,
//                      ""inputValue"": {
//                        ""text"": ""Add Distractors"",
//                        ""filledIndex"": 15,
//                        ""performance"": """",
//                        ""belong"": ""dropArea"",
//                        ""index"": 10
//                      },
//                      ""correctAns"": ""each other"",
//                      ""isGap"": true,
//                      ""incorrectIndex"": 6
//                    },
//                    {
//                      ""isTyping"": false,
//                      ""val"": "".He will help me if i got in trouble.I will help he as much as I can. I hope our friendship will "",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    },
//                    {
//                      ""status"": ""outOfChances"",
//                      ""lastIncorrectAns"": true,
//                      ""isExample"": false,
//                      ""val"": ""<span ar-gap-dd-value=\""forever and ever\"">forever and ever</span>"",
//                      ""html"": {},
//                      ""isTyping"": false,
//                      ""inputValue"": {
//                        ""text"": ""best"",
//                        ""filledIndex"": 17,
//                        ""performance"": """",
//                        ""belong"": ""dropArea"",
//                        ""index"": 4
//                      },
//                      ""correctAns"": ""forever and ever"",
//                      ""isGap"": true,
//                      ""incorrectIndex"": 7
//                    },
//                    {
//                      ""isTyping"": false,
//                      ""val"": "". A friend is someone who can let you feel warm when you are depressed. I have many friends. However I only have one best friend and it's Fisher. He is the same age as me. He is "",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    },
//                    {
//                      ""status"": ""none"",
//                      ""isExample"": true,
//                      ""val"": ""<span ar-gap-dd-value=\""taller\"" data-gap-example=\""true\"">taller</span>"",
//                      ""html"": {},
//                      ""isTyping"": false,
//                      ""inputValue"": {
//                        ""text"": ""taller""
//                      },
//                      ""correctAns"": ""taller"",
//                      ""isGap"": false
//                    },
//                    {
//                      ""isTyping"": false,
//                      ""val"": "" than me. Basketball is his favourite sport. We are in the same class. He likes studying and is good at it, so he gets high marks. We learn from each other and help each other. He will help me if I ever get into troubles. So will I. I hope our friendship will last forever and ever."",
//                      ""isGap"": false,
//                      ""html"": {},
//                      ""correctAns"": -1
//                    }
//                  ]
//                },
//                ""dropList"": [
//                  {
//                    ""text"": ""depressed"",
//                    ""filledIndex"": 9,
//                    ""performance"": """",
//                    ""belong"": ""dropArea"",
//                    ""index"": 3
//                  },
//                  {
//                    ""text"": ""taller"",
//                    ""performance"": ""none"",
//                    ""belong"": ""dropArea"",
//                    ""index"": 9
//                  },
//                  {
//                    ""text"": ""each other"",
//                    ""filledIndex"": 7,
//                    ""performance"": """",
//                    ""belong"": ""dropArea"",
//                    ""index"": 7
//                  },
//                  {
//                    ""text"": ""forever and ever"",
//                    ""filledIndex"": 1,
//                    ""performance"": """",
//                    ""belong"": ""dropArea"",
//                    ""index"": 8
//                  },
//                  {
//                    ""text"": ""favorite"",
//                    ""filledIndex"": 3,
//                    ""performance"": """",
//                    ""belong"": ""dropArea"",
//                    ""index"": 5
//                  },
//                  {
//                    ""text"": ""good"",
//                    ""filledIndex"": -1,
//                    ""performance"": ""none"",
//                    ""belong"": ""dropArea"",
//                    ""index"": 6
//                  },
//                  {
//                    ""text"": ""person-Updated"",
//                    ""filledIndex"": 11,
//                    ""performance"": """",
//                    ""belong"": ""dropArea"",
//                    ""index"": 1
//                  },
//                  {
//                    ""text"": ""warm"",
//                    ""filledIndex"": 5,
//                    ""performance"": ""none"",
//                    ""belong"": ""dropArea"",
//                    ""index"": 2
//                  },
//                  {
//                    ""text"": ""friend"",
//                    ""filledIndex"": 13,
//                    ""performance"": """",
//                    ""belong"": ""dropArea"",
//                    ""index"": 0
//                  },
//                  {
//                    ""text"": ""best"",
//                    ""filledIndex"": 17,
//                    ""performance"": """",
//                    ""belong"": ""dropArea"",
//                    ""index"": 4
//                  },
//                  {
//                    ""text"": ""Add Distractors"",
//                    ""filledIndex"": 15,
//                    ""performance"": """",
//                    ""belong"": ""dropArea"",
//                    ""index"": 10
//                  }
//                ],
//                ""transition"": ""none"",
//                ""inputMaxLength"": 16,
//                ""optionList"": [
//                  {
//                    ""text"": ""good"",
//                    ""filledIndex"": -1,
//                    ""lastIncorrectAns"": false,
//                    ""performance"": ""none"",
//                    ""belong"": ""dropArea"",
//                    ""index"": 6
//                  }
//                ]
//              }
//            ],
//            ""key"": ""cb3a910a-4253-46ff-b5b3-c084da611aef"",
//            ""snapshotKey"": ""b68baee3-02e1-475e-a20c-b15c3a326002"",
//            ""skillType"": ""Listening"",
//            ""instruction"": ""read the long text with image, fill in all the gaps.""
//          }
//        },
//        ""Effort"": null,
//        ""StudentScore"": 1.0,
//        ""TotalScore"": 9.0,
//        ""TotalQuestionCount"": 1,
//        ""CompleteQuestionCount"": 1,
//        ""CorrectQuestionCount"": 0,
//        ""AnswerType"": 0
//      },
//      {
//        ""ActivityKey"": ""95d3ec93-29d6-4615-a43e-3462ae7747d4"",
//        ""ActivitySnapshotKey"": ""0b5fbc98-dc49-4748-b02e-34227a671ba2"",
//        ""CorrectAnswers"": [
//          {
//            ""answersOrder"": []
//          }
//        ],
//        ""StudentAnswers"": {
//          ""simpleAnswers"": [
//            {
//              ""answersOrder"": []
//            }
//          ],
//          ""modelData"": {
//            ""currentPageIndex"": 0,
//            ""activityId"": ""tb16/book-3/unit-5/activities/homework/categorization-categorisation20160128"",
//            ""templateId"": ""categorization"",
//            ""customized"": true,
//            ""stimulus"": ""<p>Last summer, I went to the United States and to Italy. In the United States, I went to a city called Miami. We saw many animals there and we went to the beach every day. In Italy, I went to a city called Rome.</p>\n<p>The food in Miami was delicious. I really liked the sour soup and the burgers. The pizza in Italy was very good, but I liked the food in Miami more.</p>\n<p>The museums in Miami were modern and interesting. They weren't as big as the museums in Rome but they were nicer. Rome is a really old city, so we did a lot of sightseeing. The Opera House was amazing!</p>\n<p>Everything in Rome is older than in Miami. The buildings are old, the streets are old and even the hotels are old. Our hotel in Rome was quite small and the room was boring. We didn't even have a TV! The hotel in Miami was very cheap. You can't stay in a cheap hotel in Rome.</p>\n<p>I really liked Miami because the people are really friendly. I talked to many people in our hotel, at the beach and also on the street. I think they are nicer than in Rome. There, I didn't talk to anybody but I took many beautiful photos. </p>\n<p> </p>\n<p>1 You can see many animals there.</p>\n<p>2 In this city, you can eat pizza.</p>\n<p>3 You can visit the Oera House in this city.</p>\n<p>4  In this city, you can see old buildings.</p>\n<p>5 You can visit a museum there.</p>\n<p> </p>"",
//            ""pages"": [
//              {
//                ""reference"": 0,
//                ""transition"": ""none"",
//                ""categories"": [
//                  {
//                    ""texts"": [
//                      {
//                        ""text"": ""Rome""
//                      }
//                    ],
//                    ""answers"": [
//                      0,
//                      1,
//                      2
//                    ],
//                    ""_-_-hashKey"": ""object:115"",
//                    ""_answers"": [
//                      {
//                        ""category"": 2,
//                        ""placed"": true,
//                        ""id"": ""204"",
//                        ""droppedInCatId"": ""0"",
//                        ""isDistractor"": false,
//                        ""idx"": 4,
//                        ""text"": ""5"",
//                        ""lastMatchedCategory"": ""0"",
//                        ""isShouldMoveBackIncorrectResponse"": true
//                      },
//                      {
//                        ""category"": 1,
//                        ""placed"": true,
//                        ""id"": ""103"",
//                        ""droppedInCatId"": ""0"",
//                        ""isDistractor"": false,
//                        ""idx"": 3,
//                        ""text"": ""1"",
//                        ""lastMatchedCategory"": ""0"",
//                        ""isShouldMoveBackIncorrectResponse"": true
//                      }
//                    ],
//                    ""expanded"": false
//                  },
//                  {
//                    ""texts"": [
//                      {
//                        ""text"": ""Miami""
//                      }
//                    ],
//                    ""answers"": [
//                      3
//                    ],
//                    ""_-_-hashKey"": ""object:116"",
//                    ""_answers"": [
//                      {
//                        ""category"": 0,
//                        ""placed"": true,
//                        ""id"": ""2"",
//                        ""droppedInCatId"": ""1"",
//                        ""isDistractor"": false,
//                        ""idx"": 2,
//                        ""text"": ""4"",
//                        ""lastMatchedCategory"": ""1"",
//                        ""isShouldMoveBackIncorrectResponse"": true
//                      },
//                      {
//                        ""category"": 0,
//                        ""placed"": true,
//                        ""id"": ""1"",
//                        ""droppedInCatId"": ""1"",
//                        ""isDistractor"": false,
//                        ""idx"": 1,
//                        ""text"": ""3"",
//                        ""lastMatchedCategory"": ""2"",
//                        ""isShouldMoveBackIncorrectResponse"": true
//                      }
//                    ],
//                    ""expanded"": false
//                  },
//                  {
//                    ""texts"": [
//                      {
//                        ""text"": ""Both Cities (Rome and Miami)""
//                      }
//                    ],
//                    ""answers"": [
//                      4
//                    ],
//                    ""_-_-hashKey"": ""object:117"",
//                    ""_answers"": [
//                      {
//                        ""category"": 0,
//                        ""placed"": true,
//                        ""id"": ""0"",
//                        ""droppedInCatId"": ""2"",
//                        ""isDistractor"": false,
//                        ""idx"": 0,
//                        ""text"": ""2"",
//                        ""lastMatchedCategory"": ""1"",
//                        ""isShouldMoveBackIncorrectResponse"": true
//                      }
//                    ],
//                    ""expanded"": false
//                  }
//                ],
//                ""responseToCategoryMap"": {
//                  ""3"": 1,
//                  ""1"": 0,
//                  ""4"": 2,
//                  ""2"": 0,
//                  ""0"": 0
//                },
//                ""title"": {
//                  ""texts"": [
//                    {
//                      ""text"": ""Example from Book 3 Unit 1 Reading A""
//                    }
//                  ]
//                },
//                ""responses"": [
//                  {
//                    ""texts"": [
//                      {
//                        ""text"": ""2""
//                      }
//                    ],
//                    ""index"": 0
//                  },
//                  {
//                    ""texts"": [
//                      {
//                        ""text"": ""3""
//                      }
//                    ],
//                    ""index"": 1
//                  },
//                  {
//                    ""texts"": [
//                      {
//                        ""text"": ""4""
//                      }
//                    ],
//                    ""index"": 2
//                  },
//                  {
//                    ""texts"": [
//                      {
//                        ""text"": ""1""
//                      }
//                    ],
//                    ""index"": 3
//                  },
//                  {
//                    ""texts"": [
//                      {
//                        ""text"": ""5""
//                      }
//                    ],
//                    ""index"": 4
//                  }
//                ],
//                ""answers"": {
//                  ""all"": [
//                    {
//                      ""category"": 0,
//                      ""placed"": true,
//                      ""id"": ""0"",
//                      ""droppedInCatId"": ""2"",
//                      ""isDistractor"": false,
//                      ""idx"": 0,
//                      ""text"": ""2"",
//                      ""lastMatchedCategory"": ""1"",
//                      ""isShouldMoveBackIncorrectResponse"": true
//                    },
//                    {
//                      ""category"": 0,
//                      ""placed"": true,
//                      ""id"": ""1"",
//                      ""droppedInCatId"": ""1"",
//                      ""isDistractor"": false,
//                      ""idx"": 1,
//                      ""text"": ""3"",
//                      ""lastMatchedCategory"": ""2"",
//                      ""isShouldMoveBackIncorrectResponse"": true
//                    },
//                    {
//                      ""category"": 0,
//                      ""placed"": true,
//                      ""id"": ""2"",
//                      ""droppedInCatId"": ""1"",
//                      ""isDistractor"": false,
//                      ""idx"": 2,
//                      ""text"": ""4"",
//                      ""lastMatchedCategory"": ""1"",
//                      ""isShouldMoveBackIncorrectResponse"": true
//                    },
//                    {
//                      ""category"": 1,
//                      ""placed"": true,
//                      ""id"": ""103"",
//                      ""droppedInCatId"": ""0"",
//                      ""isDistractor"": false,
//                      ""idx"": 3,
//                      ""text"": ""1"",
//                      ""lastMatchedCategory"": ""0"",
//                      ""isShouldMoveBackIncorrectResponse"": true
//                    },
//                    {
//                      ""category"": 2,
//                      ""placed"": true,
//                      ""id"": ""204"",
//                      ""droppedInCatId"": ""0"",
//                      ""isDistractor"": false,
//                      ""idx"": 4,
//                      ""text"": ""5"",
//                      ""lastMatchedCategory"": ""0"",
//                      ""isShouldMoveBackIncorrectResponse"": true
//                    }
//                  ],
//                  ""unplaced"": []
//                },
//                ""_isShowingNextBar"": true,
//                ""completed"": true,
//                ""state"": 4,
//                ""_-_-hashKey"": ""object:106""
//              }
//            ],
//            ""stimulusHtml"": {},
//            ""key"": ""95d3ec93-29d6-4615-a43e-3462ae7747d4"",
//            ""snapshotKey"": ""0b5fbc98-dc49-4748-b02e-34227a671ba2"",
//            ""skillType"": ""Listening"",
//            ""instruction"": ""Categorisation: Text stimulus, Text responses""
//          }
//        },
//        ""Effort"": null,
//        ""StudentScore"": 0.0,
//        ""TotalScore"": 5.0,
//        ""TotalQuestionCount"": 1,
//        ""CompleteQuestionCount"": 1,
//        ""CorrectQuestionCount"": 0,
//        ""AnswerType"": 0
//      }
//    ]
//  },
//  ""operatorKey"": ""3c40ab54-798c-4517-a82a-26017ee98285""
//}");
//            var xml = JsonXmlizer.Xmlize(jtoken);

//            Assert.IsNotNull(xml);
//        }
//    }
//}