using System;
using System.Diagnostics;

namespace NullFight
{
    public partial struct Option
    {
        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values. It will throw an exception if any of them are missing a value.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2) GetValueOrThrow<T1, T2>(Option<T1> option1, Option<T2> option2, string exceptionMessage = null)
        {
            return (option1.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option2.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"));
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values. It will throw an exception if any of them are missing a value.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3) GetValueOrThrow<T1, T2, T3>(Option<T1> option1, Option<T2> option2, Option<T3> option3, string exceptionMessage = null)
        {
            return (option1.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option2.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option3.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"));
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values. It will throw an exception if any of them are missing a value.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4) GetValueOrThrow<T1, T2, T3, T4>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, string exceptionMessage = null)
        {
            return (option1.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option2.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option3.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option4.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"));
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values. It will throw an exception if any of them are missing a value.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4, T5) GetValueOrThrow<T1, T2, T3, T4, T5>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, Option<T5> option5, string exceptionMessage = null)
        {
            return (option1.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option2.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option3.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option4.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option5.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"));
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values. It will throw an exception if any of them are missing a value.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4, T5, T6) GetValueOrThrow<T1, T2, T3, T4, T5, T6>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, Option<T5> option5, Option<T6> option6, string exceptionMessage = null)
        {
            return (option1.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option2.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option3.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option4.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option5.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option6.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"));
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values. It will throw an exception if any of them are missing a value.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4, T5, T6, T7) GetValueOrThrow<T1, T2, T3, T4, T5, T6, T7>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, Option<T5> option5, Option<T6> option6, Option<T7> option7, string exceptionMessage = null)
        {
            return (option1.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option2.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option3.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option4.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option5.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option6.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option7.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"));
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values. It will throw an exception if any of them are missing a value.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8) GetValueOrThrow<T1, T2, T3, T4, T5, T6, T7, T8>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, Option<T5> option5, Option<T6> option6, Option<T7> option7, Option<T8> option8, string exceptionMessage = null)
        {
            return (option1.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option2.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option3.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option4.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option5.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option6.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option7.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option8.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"));
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values. It will throw an exception if any of them are missing a value.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) GetValueOrThrow<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, Option<T5> option5, Option<T6> option6, Option<T7> option7, Option<T8> option8, Option<T9> option9, string exceptionMessage = null)
        {
            return (option1.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option2.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option3.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option4.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option5.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option6.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option7.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option8.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"), option9.GetValueOrThrow(exceptionMessage ?? "Value not present in Option"));
        }


        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values event if they are None(). Nulls will need to be checked.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2) GetValues<T1, T2>(Option<T1> option1, Option<T2> option2)
        {
            return (option1.Value, option2.Value);
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values event if they are None(). Nulls will need to be checked.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3) GetValues<T1, T2, T3>(Option<T1> option1, Option<T2> option2, Option<T3> option3)
        {
            return (option1.Value, option2.Value, option3.Value);
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values event if they are None(). Nulls will need to be checked.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4) GetValues<T1, T2, T3, T4>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4)
        {
            return (option1.Value, option2.Value, option3.Value, option4.Value);
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values event if they are None(). Nulls will need to be checked.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4, T5) GetValues<T1, T2, T3, T4, T5>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, Option<T5> option5)
        {
            return (option1.Value, option2.Value, option3.Value, option4.Value, option5.Value);
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values event if they are None(). Nulls will need to be checked.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4, T5, T6) GetValues<T1, T2, T3, T4, T5, T6>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, Option<T5> option5, Option<T6> option6)
        {
            return (option1.Value, option2.Value, option3.Value, option4.Value, option5.Value, option6.Value);
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values event if they are None(). Nulls will need to be checked.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4, T5, T6, T7) GetValues<T1, T2, T3, T4, T5, T6, T7>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, Option<T5> option5, Option<T6> option6, Option<T7> option7)
        {
            return (option1.Value, option2.Value, option3.Value, option4.Value, option5.Value, option6.Value, option7.Value);
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values event if they are None(). Nulls will need to be checked.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8) GetValues<T1, T2, T3, T4, T5, T6, T7, T8>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, Option<T5> option5, Option<T6> option6, Option<T7> option7, Option<T8> option8)
        {
            return (option1.Value, option2.Value, option3.Value, option4.Value, option5.Value, option6.Value, option7.Value, option8.Value);
        }

        /// <summary>
        /// Takes a list of Options and returns a single tuple containing all of their values event if they are None(). Nulls will need to be checked.
        /// </summary>
        /// <returns></returns>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) GetValues<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Option<T1> option1, Option<T2> option2, Option<T3> option3, Option<T4> option4, Option<T5> option5, Option<T6> option6, Option<T7> option7, Option<T8> option8, Option<T9> option9)
        {
            return (option1.Value, option2.Value, option3.Value, option4.Value, option5.Value, option6.Value, option7.Value, option8.Value, option9.Value);
        }

    }
}
