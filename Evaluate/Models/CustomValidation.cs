using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluate.Models
{
    public class OnlyOneAnswer : ValidationAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if ((bool)value)
                {
                    var model = (Models.Answer)validationContext.ObjectInstance;
                    //int _questionId = Convert.ToInt32(model.QuestionId);                
                    int _answerId = Convert.ToInt32(model.AnswerId);
                    int _questionId;
                    if (_answerId != 0)
                    {
                        _questionId = Convert.ToInt32((from a in db.Answers
                                           where a.AnswerId == _answerId
                                           select a.QuestionId).FirstOrDefault());
                    }
                    else
                    {
                        _questionId = Convert.ToInt32(model.QuestionId);
                    }

                    var correctAnswer = db.Answers.Where(a => a.QuestionId == _questionId && a.IsCorrect == true).FirstOrDefault();

                    if (correctAnswer != null)
                    {
                        if (_answerId == correctAnswer.AnswerId)
                        {
                            return ValidationResult.Success;
                        }
                        else
                        {
                            return new ValidationResult("Ya existe una respuesta correcta para esta pregunta.");
                        }
                    }
                }
            }
            return ValidationResult.Success;
        }
    }

    //public class OneAnswerAttribute : ValidationAttribute
    //{
    //    public bool ExistsOneAnswer { get; set; }

    //    public override bool IsValid(object value)
    //    {
    //        if (value == null)
    //        {
    //            return true;
    //        }
    //        var answer = (bool)value;

    //        //if (price < MinPrice)
    //        //{
    //        //    return false;
    //        //}
    //        //double cents = price - Math.Truncate(price);
    //        //if (cents < 0.99 || cents >= 0.995)
    //        //{
    //        //    return false;
    //        //}

    //        return true;
    //    }
    //}
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //public sealed class OneAnswerAttribute : ValidationAttribute
    //{
    //    private ApplicationDbContext db = new ApplicationDbContext();

    //    private const string DefaultErrorMessage = "Ya existe una respuesta correcta para esta pregunta.";

    //    public string OtherProperty { get; private set; }

    //    public OneAnswerAttribute(string otherProperty)
    //        : base(DefaultErrorMessage)
    //    {
    //        if (string.IsNullOrEmpty(otherProperty))
    //        {
    //            throw new ArgumentNullException("otherProperty");
    //        }

    //        OtherProperty = otherProperty;
    //    }

    //    public override string FormatErrorMessage(string name)
    //    {
    //        return string.Format(ErrorMessageString, name, OtherProperty);
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        if (value != null)
    //        {
    //            var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty);

    //            var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

    //            if ((bool)value)
    //            {
    //                var answer = db.Answers.Where(a => a.AnswerId == (int)otherPropertyValue).Single();

    //                if (answer.IsCorrect != true)
    //                {
    //                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    //                }
    //            }

    //            //if (value.Equals(otherPropertyValue))
    //            //{
    //            //    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    //            //}
    //        }

    //        return ValidationResult.Success;
    //    }
    //}    
}