using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FolderIconChangerWPF.AttachedProperties
{
    public static partial class TextBoxHelper
    {
        #region EzzTextBox

        #region Enums

        public enum EditTypes
        {
            Normal = 0,
            Numeric = 1,
            FileName = 2,
            Path = 3
        }
        public enum NumericTypes
        {
            AcceptAny = 0,
            DoNotAcceptDecimalSymbol = 1,
            DoNotAcceptSign = 2,
            DoNotAcceptBoth = 3
        }
        public enum EditOperations
        {
            CText,
            CSelectedText,
            BackSpace,
            Delete,
            Cut,
            Paste,
            Other
        }

        #endregion

        public static bool GetEnableEzzTextBox(DependencyObject obj) => (bool)obj.GetValue(EnableEzzTextBoxProperty);

        public static void SetEnableEzzTextBox(DependencyObject obj, bool value) => obj.SetValue(EnableEzzTextBoxProperty, value);

        // Using a DependencyProperty as the backing store for EnableEzzTextBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableEzzTextBoxProperty =
            DependencyProperty.RegisterAttached("EnableEzzTextBox", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, new PropertyChangedCallback(OnEnableEzzTextBoxPropertyChanged)));

        private static void OnEnableEzzTextBoxPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is TextBox textBox))
            {
                textBox.PreviewTextInput -= PreviewTextInputHandler;
                textBox.PreviewKeyDown -= PreviewKeyDownHandler;
                //DataObject.RemovePastingHandler(AssociatedObject, PastingHandler);
                CommandManager.RemovePreviewExecutedHandler(textBox, PreviewExecutedHandler);
                //textBox.GotFocus -= this.TextBox_GotFocus;
                textBox.GotKeyboardFocus -= TextBox_GotKeyboardFocus;
                //textBox.GotMouseCapture -= TextBox_GotMouseCapture;
                textBox.GotTouchCapture -= TextBox_GotTouchCapture;
                if (e.NewValue is bool && (bool)e.NewValue)
                {
                    textBox.PreviewTextInput += PreviewTextInputHandler;
                    textBox.PreviewKeyDown += PreviewKeyDownHandler;
                    //DataObject.AddPastingHandler(textBox, PastingHandler);
                    CommandManager.AddPreviewExecutedHandler(textBox, PreviewExecutedHandler);
                    //textBox.GotFocus += this.TextBox_GotFocus;
                    textBox.GotKeyboardFocus += TextBox_GotKeyboardFocus;
                    //textBox.GotMouseCapture += TextBox_GotMouseCapture;
                    //textBox.GotTouchCapture += TextBox_GotTouchCapture;

                    SetCharsChecker(d, new CharsChecker());
                }
            }
            else if ((d is ComboBox comboBox))
            {
                comboBox.PreviewTextInput -= PreviewTextInputHandler;
                comboBox.PreviewKeyDown -= PreviewKeyDownHandler;
                //DataObject.RemovePastingHandler(AssociatedObject, PastingHandler);
                CommandManager.RemovePreviewExecutedHandler(comboBox, PreviewExecutedHandler);
                //textBox.GotFocus -= this.TextBox_GotFocus;
                comboBox.GotKeyboardFocus -= TextBox_GotKeyboardFocus;
                //comboBox.GotMouseCapture -= TextBox_GotMouseCapture;
                comboBox.GotTouchCapture -= TextBox_GotTouchCapture;
                if (e.NewValue is bool && (bool)e.NewValue)
                {
                    comboBox.PreviewTextInput += PreviewTextInputHandler;
                    comboBox.PreviewKeyDown += PreviewKeyDownHandler;
                    //DataObject.AddPastingHandler(textBox, PastingHandler);
                    CommandManager.AddPreviewExecutedHandler(comboBox, PreviewExecutedHandler);
                    //textBox.GotFocus += this.TextBox_GotFocus;
                    comboBox.GotKeyboardFocus += TextBox_GotKeyboardFocus;
                    //comboBox.GotMouseCapture += TextBox_GotMouseCapture;
                    //comboBox.GotTouchCapture += TextBox_GotTouchCapture;

                    SetCharsChecker(d, new CharsChecker());
                }
            }
            //else
            //{
            //    RemoveOldHandles();
            //}

        }
        static void SelectAllMethod(object sender)
        {
            if ((sender is TextBox textBox))
            {
                if (!GetSelectAllOnFocus(textBox)) return;
                textBox.SelectAll();
            }
            else if (sender is ComboBox comboBox)
            {
                //if (!GetSelectAllOnFocus(comboBox)) return;
                //comboBox.
            }
        }
        private static void TextBox_GotTouchCapture(object sender, TouchEventArgs e) => SelectAllMethod(sender);
        private static void TextBox_GotMouseCapture(object sender, MouseEventArgs e) => SelectAllMethod(sender);
        private static void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => SelectAllMethod(sender);

        //
        //private static void PreviewExecutedHandler(object sender, ExecutedRoutedEventArgs e) => throw new NotImplementedException();
        //private static void PreviewKeyDownHandler(object sender, KeyEventArgs e) => throw new NotImplementedException();
        //private static void PreviewTextInputHandler(object sender, TextCompositionEventArgs e) => throw new NotImplementedException();


        #region Ezz TextBox Props

        public static EditTypes GetEditType(DependencyObject obj) => (EditTypes)obj.GetValue(EditTypeProperty);

        public static void SetEditType(DependencyObject obj, EditTypes value) => obj.SetValue(EditTypeProperty, value);

        // Using a DependencyProperty as the backing store for EditType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditTypeProperty =
            DependencyProperty.RegisterAttached("EditType", typeof(EditTypes), typeof(TextBoxHelper), new PropertyMetadata(EditTypes.Normal, new PropertyChangedCallback(OnEditTypePropertyChanged)));

        private static void OnEditTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CheckToEnableEzzTextBox(d);
            UpdateLists(d);
        }

        public static NumericTypes GetNumericType(DependencyObject obj) => (NumericTypes)obj.GetValue(NumericTypeProperty);

        public static void SetNumericType(DependencyObject obj, NumericTypes value) => obj.SetValue(NumericTypeProperty, value);

        // Using a DependencyProperty as the backing store for NumericType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumericTypeProperty =
            DependencyProperty.RegisterAttached("NumericType", typeof(NumericTypes), typeof(TextBoxHelper), new PropertyMetadata(NumericTypes.AcceptAny, new PropertyChangedCallback(OnNumericTypePropertyChanged)));

        private static void OnNumericTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CheckToEnableEzzTextBox(d);
            UpdateLists(d);
        }

        public static decimal GetMinNumber(DependencyObject obj) => (decimal)obj.GetValue(MinNumberProperty);

        public static void SetMinNumber(DependencyObject obj, decimal value) => obj.SetValue(MinNumberProperty, value);

        // Using a DependencyProperty as the backing store for MinNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinNumberProperty =
            DependencyProperty.RegisterAttached("MinNumber", typeof(decimal), typeof(TextBoxHelper), new PropertyMetadata(decimal.MinValue, new PropertyChangedCallback(OnMinNumberPropertyChanged)));

        private static void OnMinNumberPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => CheckToEnableEzzTextBox(d);


        public static decimal GetMaxNumber(DependencyObject obj) => (decimal)obj.GetValue(MaxNumberProperty);

        public static void SetMaxNumber(DependencyObject obj, decimal value) => obj.SetValue(MaxNumberProperty, value);

        // Using a DependencyProperty as the backing store for MaxNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxNumberProperty =
            DependencyProperty.RegisterAttached("MaxNumber", typeof(decimal), typeof(TextBoxHelper), new PropertyMetadata(decimal.MaxValue, new PropertyChangedCallback(OnMaxNumberPropertyChanged)));

        private static void OnMaxNumberPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => CheckToEnableEzzTextBox(d);


        public static bool GetAllowToTogglesTheSign(DependencyObject obj) => (bool)obj.GetValue(AllowToTogglesTheSignProperty);

        public static void SetAllowToTogglesTheSign(DependencyObject obj, bool value) => obj.SetValue(AllowToTogglesTheSignProperty, value);

        // Using a DependencyProperty as the backing store for AllowToTogglesTheSign.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllowToTogglesTheSignProperty =
            DependencyProperty.RegisterAttached("AllowToTogglesTheSign", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, new PropertyChangedCallback(OnAllowToTogglesTheSignPropertyChanged)));

        private static void OnAllowToTogglesTheSignPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => CheckToEnableEzzTextBox(d);


        public static bool GetAllowToCalculate(DependencyObject obj) => (bool)obj.GetValue(AllowToCalculateProperty);

        public static void SetAllowToCalculate(DependencyObject obj, bool value) => obj.SetValue(AllowToCalculateProperty, value);

        // Using a DependencyProperty as the backing store for AllowToCalculate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllowToCalculateProperty =
            DependencyProperty.RegisterAttached("AllowToCalculate", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, new PropertyChangedCallback(OnAllowToCalculatePropertyChanged)));

        private static void OnAllowToCalculatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => CheckToEnableEzzTextBox(d);


        public static bool GetFreeInputForCalculating(DependencyObject obj) => (bool)obj.GetValue(FreeInputForCalculatingProperty);

        public static void SetFreeInputForCalculating(DependencyObject obj, bool value) => obj.SetValue(FreeInputForCalculatingProperty, value);

        // Using a DependencyProperty as the backing store for FreeInputForCalculating.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FreeInputForCalculatingProperty =
            DependencyProperty.RegisterAttached("FreeInputForCalculating", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, new PropertyChangedCallback(OnFreeInputForCalculatingPropertyChanged)));

        private static void OnFreeInputForCalculatingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => CheckToEnableEzzTextBox(d);



        public static bool GetSelectAllOnFocus(DependencyObject obj) => (bool)obj.GetValue(SelectAllOnFocusProperty);

        public static void SetSelectAllOnFocus(DependencyObject obj, bool value) => obj.SetValue(SelectAllOnFocusProperty, value);

        // Using a DependencyProperty as the backing store for SelectAllOnFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectAllOnFocusProperty =
            DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, new PropertyChangedCallback(OnSelectAllOnFocusPropertyChanged)));

        private static void OnSelectAllOnFocusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => CheckToEnableEzzTextBox(d);

        static HashSet<CharWithProperties> AllowedChars(DependencyObject d) => GetCharsChecker(d)?.AllowedChars;
        static HashSet<CharWithProperties> NotAllowedChars(DependencyObject d) => GetCharsChecker(d)?.NotAllowedChars;

        #endregion

        #region private props


        static CharsChecker GetCharsChecker(DependencyObject obj) => (CharsChecker)obj.GetValue(CharsCheckerProperty);

        static void SetCharsChecker(DependencyObject obj, CharsChecker value) => obj.SetValue(CharsCheckerProperty, value);

        // Using a DependencyProperty as the backing store for CharsChecker.  This enables animation, styling, binding, etc...
        static readonly DependencyProperty CharsCheckerProperty =
            DependencyProperty.RegisterAttached("CharsChecker", typeof(CharsChecker), typeof(TextBoxHelper), new PropertyMetadata(null));



        static TextBox GetCTextBox(DependencyObject obj) => (TextBox)obj.GetValue(CTextBoxProperty);

        static void SetCTextBox(DependencyObject obj, TextBox value) => obj.SetValue(CTextBoxProperty, value);

        // Using a DependencyProperty as the backing store for CTextBox.  This enables animation, styling, binding, etc...
        static readonly DependencyProperty CTextBoxProperty =
            DependencyProperty.RegisterAttached("CTextBox", typeof(TextBox), typeof(TextBoxHelper), new PropertyMetadata(null));



        #endregion



        #region DP Methods

        static void CheckToEnableEzzTextBox(DependencyObject d)
        {
            if (!GetEnableEzzTextBox(d)) SetEnableEzzTextBox(d, true);
        }


        #endregion

        #region Methods

        //public event InputEventHandler NotAllowedInput;
        static void OnNotAllowedInput(InputEventArgs e)
        {
            System.Media.SystemSounds.Beep.Play();
            //if (NotAllowedInput != null) NotAllowedInput(this, e);
        }

        //public event InputEventHandler AllowedInput;
        static void OnAllowedInput(InputEventArgs e) { /*if (AllowedInput != null) AllowedInput(this, e);*/ }

        //public event TextChangingEventHandler TextChanging;
        static void OnTextChanging(TextChangingEventArgs e) { /*if (TextChanging != null) TextChanging(this, e);*/ }

        #region Private Methods

        static TextChangingEventArgs RaiseTextChangingEvent_NCheck(DependencyObject d, EditOperations OperationIs, string theTransText = "")
        {
            return RaiseTextChangingEvent_NCheck(d, GetTextChangingEventArgs(d, OperationIs, theTransText));
        }

        static TextChangingEventArgs RaiseTextChangingEvent_NCheck(DependencyObject d, TextChangingEventArgs NewTC)
        {
            if (!(d is TextBox textBox)) return NewTC;

            if (NewTC.TheTransferText.IsNullOrEmpty())
            {
                NewTC.Cancel = true;
            }
            else
            {
                CheckTextChanging(d, NewTC);
            }
            if (!NewTC.Cancel)
            {
                OnTextChanging(NewTC);
                if (!NewTC.Cancel)
                {
                    textBox.Text = NewTC.TextAfterTheChange;
                    textBox.SelectionStart = NewTC.SelectionStart;
                }
            }

            return NewTC;
        }

        //

        static TextBox _CTextBox_SetCurrInfo(DependencyObject d)
        {
            if (!(d is TextBox textBox)) return null;
            TextBox _CTextBox = GetCTextBox(d);
            if (_CTextBox == null)
            {
                _CTextBox = new TextBox();
                SetCTextBox(d, _CTextBox);
            }

            _CTextBox.Text = textBox.Text;
            _CTextBox.SelectionStart = textBox.SelectionStart;
            _CTextBox.SelectionLength = textBox.SelectionLength;
            return _CTextBox;
        }

        static TextChangingEventArgs GetTextChangingEventArgs(DependencyObject d, EditOperations OperationIs, string theTransText = "")
        {
            TextBox _CTextBox = _CTextBox_SetCurrInfo(d);
            string TBefore = _CTextBox.Text;
            switch (OperationIs)
            {
                case EditOperations.CText:
                case EditOperations.Other:
                    _CTextBox.Text = theTransText;
                    break;

                case EditOperations.CSelectedText:
                    var oldSelectedLen = _CTextBox.SelectedText.Length;
                    _CTextBox.SelectedText = theTransText;
                    _CTextBox.SelectionStart = Math.Max(_CTextBox.SelectionStart - oldSelectedLen, 0);
                    _CTextBox.SelectionStart += theTransText?.Length ?? 0;
                    break;

                case EditOperations.BackSpace:
                    if (_CTextBox.SelectedText.IsNullOrEmpty())
                    {
                        if (_CTextBox.Text.Length != 0 && _CTextBox.SelectionStart != 0)
                        {
                            int NewSStart = _CTextBox.SelectionStart - 1;
                            theTransText = _CTextBox.Text.Substring(NewSStart, 1);//_CTextBox.Text[NewSStart].ToString();
                            _CTextBox.Text = _CTextBox.Text.Remove(NewSStart, 1);
                            _CTextBox.SelectionStart = NewSStart;
                        }
                    }
                    else
                    {
                        theTransText = _CTextBox.SelectedText;
                        _CTextBox.SelectedText = string.Empty;
                    }
                    break;

                case EditOperations.Delete:
                    if (_CTextBox.SelectedText.IsNullOrEmpty())
                    {
                        if (_CTextBox.Text.Length != 0 && _CTextBox.SelectionStart < _CTextBox.Text.Length)
                        {
                            int NewSStart = _CTextBox.SelectionStart;
                            theTransText = _CTextBox.Text.Substring(NewSStart, 1);//_CTextBox.Text[NewSStart].ToString();
                            _CTextBox.Text = _CTextBox.Text.Remove(NewSStart, 1);
                            _CTextBox.SelectionStart = NewSStart;
                        }
                    }
                    else
                    {
                        theTransText = _CTextBox.SelectedText;
                        _CTextBox.SelectedText = string.Empty;
                    }
                    break;

                case EditOperations.Cut:
                    if (!_CTextBox.SelectedText.IsNullOrEmpty())
                    {
                        theTransText = _CTextBox.SelectedText;
                        _CTextBox.SelectedText = string.Empty;
                    }
                    break;

                case EditOperations.Paste:
                    theTransText = Clipboard.GetText();
                    _CTextBox.SelectedText = theTransText;
                    _CTextBox.SelectionStart += theTransText.Length;
                    break;
            }
            return new TextChangingEventArgs(_CTextBox, theTransText, TBefore, _CTextBox.Text, _CTextBox.SelectionStart, OperationIs);
        }

        //
        static void ClearLists(DependencyObject d)
        {
            switch (GetEditType(d))
            {
                case EditTypes.Numeric:
                    AllowedChars(d)?.RemoveChars(NumericValues(d));
                    break;

                case EditTypes.FileName:
                    NotAllowedChars(d)?.RemoveChars(Path.GetInvalidFileNameChars());
                    break;

                case EditTypes.Path:
                    NotAllowedChars(d)?.RemoveChars(Path.GetInvalidPathChars());
                    break;
                    //case EditTypes.Normal:
                    //default:
                    //    break;
            }
        }

        static void UpdateLists(DependencyObject d, bool clear = true)
        {
            if (clear) ClearLists(d);
            switch (GetEditType(d))
            {
                case EditTypes.Numeric:
                    AllowedChars(d)?.UnionWith(NumericValues(d));
                    break;

                case EditTypes.FileName:
                    NotAllowedChars(d)?.AddChars(Path.GetInvalidFileNameChars(), 0);
                    break;

                case EditTypes.Path:
                    NotAllowedChars(d)?.AddChars(Path.GetInvalidPathChars(), 0);
                    break;
                    //case EditTypes.Normal:
                    //default:
                    //    break;
            }
        }

        static HashSet<CharWithProperties> NumericValues(DependencyObject d)
        {
            var newL = new HashSet<CharWithProperties>();
            for (int i = 0; i <= 9; i++)
            {
                newL.AddChar(i.ToString()[0]);
            }
            //if (AllowToCalculate)
            //{
            //    if (NumericType == NumericTypes.AcceptAny || NumericType == NumericTypes.DoNotAcceptSign) newL.AddChar('.', 1);
            //    newL.AddChars(Operators);
            //}
            //else
            //{
            switch (GetNumericType(d))
            {
                case NumericTypes.AcceptAny:
                    newL.AddChars(new char[] { '.', '-', '+' }, 1);
                    break;

                case NumericTypes.DoNotAcceptDecimalSymbol:
                    newL.AddChars(new char[] { '-', '+' }, 1);
                    break;

                case NumericTypes.DoNotAcceptSign:
                    newL.AddChars(new char[] { '.' }, 1);
                    break;
                    //case NumericTypes.DoNotAcceptBoth:
                    //    break;
            }
            //}

            return newL;
        }

        //

        static void CheckTextChanging(DependencyObject d, TextChangingEventArgs e)
        {
            if (!(d is TextBox textBox)) return;

            var EditType = GetEditType(d);
            var AllowToCalculate = GetAllowToCalculate(d);
            var FreeInputForCalculating = GetFreeInputForCalculating(d);
            if (EditType == EditTypes.Numeric && AllowToCalculate && FreeInputForCalculating)
            {
                if (!AllowedChars(d).StringContainsUnlistedChars(e.TheTransferText, OperatorsNBrackets()))
                {
                    goto finish;
                }
            }
            if (!GetCharsChecker(d).StringIsAllowed(e.TextAfterTheChange))
            {
                //
                if (EditType == EditTypes.Numeric)
                {
                    if ((!FreeInputForCalculating) && (e.TheTransferText == "-" || e.TheTransferText == "+")) goto nextCheck;
                    if (AllowToCalculate)
                    {
                        //if (CheckToAddNumber(e.TheTransferText, e.TextAfterTheChange)
                        //    || e.TextAfterTheChange.StartsWithAnyChar(Operators)
                        //    || e.TextAfterTheChange.EndsWithAnyChar(Operators))//e.TextAfterTheChange.CanEvaluate()
                        //{
                        //    goto finish;
                        //}
                        //else
                        if ((e.TheTransferText == "(" || e.TheTransferText == ")") && !IsAnOperationOfTwoBrackets(textBox.Text))
                        {
                            e.TextAfterTheChange = "(" + e.TextAfterTheChange.Replace("(", "").Replace(")", "") + ")";
                            e.SelectionStart = e.TextAfterTheChange.Length - 1;
                            goto finish;
                        }
                        else if (e.TheTransferText == "()" && IsAnOperationOfTwoBrackets(textBox.Text))
                        {
                            e.SelectionStart -= 1;
                            goto finish;
                        }
                        else if (e.TheTransferText == "(" || e.TheTransferText == ")" && IsAnOperationOfTwoBrackets(textBox.Text))
                        {
                            if (e.OperationIsInput())
                            {
                                e.TheTransferText = "()";
                                e.SelectionStart -= 1;
                                goto finish;
                            }
                            else// if(e.OperationIsDelete())
                            {
                                if (e.TextAfterTheChange == "(" || e.TextAfterTheChange == ")")
                                {
                                    e.TextAfterTheChange = "";
                                    goto nextCheck;
                                }
                                string RemovedB = "";
                                RemovedB = textBox.Text.Remove(0, 1);
                                RemovedB = RemovedB.Remove(RemovedB.Length - 1, 1);

                                if (RemovedB.IsNumeric())
                                {
                                    e.TextAfterTheChange = RemovedB;
                                    e.SelectionStart = e.TextAfterTheChange.Length;
                                    goto nextCheck;
                                }
                                else if (e.TheTransferText == "(")
                                {
                                    int DSS = textBox.SelectionStart == 0 ? 0 : textBox.SelectionStart - 1;

                                    //int Pos_1 = -1;
                                    //if (textBox.Text[DSS] == ')') { Pos_1 = 0; }

                                    int Pos_2 = 0;
                                    bool cl = true;
                                    for (int i = DSS; i < textBox.Text.Length; i++)
                                    {
                                        char ch = textBox.Text[i];
                                        if (ch == '(')
                                        {
                                            cl = false;
                                        }
                                        else if (ch == ')')
                                        {
                                            if (cl)
                                            {
                                                Pos_2 = i;
                                                break;
                                            }
                                            cl = true;
                                        }
                                    }
                                    if (Pos_2 != 0) Pos_2 -= 1;

                                    int newSS = e.SelectionStart;
                                    e.TextAfterTheChange = e.TextAfterTheChange.Remove(Pos_2, 1);
                                    e.SelectionStart = newSS;
                                }
                                else //if (e.TheTransferText == ")")
                                {
                                    int DSS = e.OperationIs == EditOperations.BackSpace ? textBox.SelectionStart - 2 : textBox.SelectionStart - 1;
                                    int Pos_1 = 0;
                                    //int le = textBox.Text.Length;
                                    bool cl = true;
                                    for (int i = DSS; i >= 0; i--)
                                    {
                                        char ch = textBox.Text[i];
                                        if (ch == ')')
                                        {
                                            cl = false;
                                        }
                                        else if (ch == '(')
                                        {
                                            if (cl)
                                            {
                                                Pos_1 = i;
                                                break;
                                            }
                                            cl = true;
                                        }
                                    }
                                    int newSS = e.SelectionStart;
                                    e.TextAfterTheChange = e.TextAfterTheChange.Remove(Pos_1, 1);
                                    e.SelectionStart = newSS - 1;
                                }
                                //
                                //e.TextAfterTheChange = textBox.Text.Replace("()", "");
                                //e.SelectionStart = textBox.SelectionStart - (textBox.Text.Length - e.TextAfterTheChange.Length);
                                goto nextCheck;
                            }
                        }
                        else if (e.TextAfterTheChange == "(" || e.TextAfterTheChange == ")")
                        {
                            e.TextAfterTheChange = "";
                            goto nextCheck;
                        }
                        if (IsAnOperationOfTwoBrackets(e.TextAfterTheChange))
                        {
                            if (!ContainsInvaildNumValues(d, e.TheTransferText)) goto nextCheck;
                        }
                    }
                    if ((!e.TheTransferText.IsNullOrEmpty() && ContainsOperator(e.TheTransferText)) && (e.OperationIsInput()))
                    {
                        try
                        {
                            string res = e.TheTransferText.EvaluateString();
                            if (res.IsNumeric())
                            {
                                e.TheTransferText = res;
                                goto nextCheck;
                            }
                        }
                        catch { }
                    }
                    if (!GetAllowToTogglesTheSign(d) && e.TheTransferText == "-")
                    {
                        goto nextCheck;
                    }
                }
                //
                //NotAllowed:
                e.Cancel = true;
                OnNotAllowedInput(new InputEventArgs(e.TheTransferText, e.TextAfterTheChange));
                return;
            }
        nextCheck:
            if (EditType == EditTypes.Numeric)
            {
                if (!IsAnOperationOfTwoBrackets(e.TextAfterTheChange))
                {
                    if ((!e.TheTransferText.IsNullOrEmpty() && ContainsOperator(e.TheTransferText)) && (e.OperationIsInput()))
                    {
                        try
                        {
                            string res = e.TheTransferText.EvaluateString();
                            if (res.IsNumeric())
                            {
                                e.TheTransferText = res;
                            }
                        }
                        catch { }
                    }
                    //
                    //if (AllowToCalculate && ContainsOperator(textBox.Text))
                    //{
                    //    //
                    //}
                    //else
                    //{
                    if (GetAllowToTogglesTheSign(d))
                    {
                        if (e.TheTransferText == ("-"))
                        {
                            int PreSS = e.SelectionStart;
                            if (textBox.Text.Contains("-"))
                            {
                                e.TextAfterTheChange = e.TextAfterTheChange.Replace("+", "").Replace("-", "");
                                e.SelectionStart = PreSS - 1;
                            }
                            else
                            {
                                e.TextAfterTheChange = "-" + e.TextAfterTheChange.Replace("+", "").Replace("-", "");
                                e.SelectionStart = PreSS + 1;
                            }
                        }
                        else if (e.TheTransferText == ("+"))
                        {
                            if (textBox.Text.Contains("-"))
                            {
                                int PreSS = e.SelectionStart;
                                e.TextAfterTheChange = e.TextAfterTheChange.Replace("+", "").Replace("-", "");
                                e.SelectionStart = PreSS - 1;
                            }
                            else
                            {
                                e.Cancel = true;
                                OnNotAllowedInput(new InputEventArgs(e.TheTransferText, e.TextAfterTheChange));
                                return;
                            }
                        }
                    }

                    //Check zero
                    if ((!FreeInputForCalculating) && !(ContainsOperator(e.TheTransferText)))
                    {
                        string str = e.TextAfterTheChange;
                        int Pos = e.SelectionStart;
                        CheckZero(ref str, ref Pos);
                        e.TextAfterTheChange = str;
                        e.SelectionStart = Pos;
                    }
                    //}
                    //
                    if (!CheckToAddNumber(d, e.TheTransferText, e.TextAfterTheChange))
                    {
                        if (!GetAllowToTogglesTheSign(d) && e.TheTransferText == "-" && e.SelectionStart == 1)
                        {
                            if (e.TextAfterTheChange.StartsWith("+", StringComparison.Ordinal))
                            {
                                int PreSS = e.SelectionStart;
                                e.TextAfterTheChange = "-" + e.TextAfterTheChange.Replace("+", "").Replace("-", "");
                                e.SelectionStart = PreSS;
                                goto finish;
                            }
                            else if (e.TextAfterTheChange.StartsWith("-", StringComparison.Ordinal))
                            {
                                int PreSS = e.SelectionStart;
                                e.TextAfterTheChange = e.TextAfterTheChange.Replace("+", "").Replace("-", "");
                                e.SelectionStart = PreSS - 1;
                                goto finish;
                            }
                        }
                        else if (!GetAllowToTogglesTheSign(d) && e.TheTransferText == "+" && e.SelectionStart == 1 && e.TextAfterTheChange.StartsWith("-", StringComparison.Ordinal))
                        {
                            int PreSS = e.SelectionStart;
                            e.TextAfterTheChange = e.TextAfterTheChange.Replace("+", "").Replace("-", "");
                            e.SelectionStart = PreSS - 1;
                            goto finish;
                        }
                        e.Cancel = true;
                        OnNotAllowedInput(new InputEventArgs(e.TheTransferText, e.TextAfterTheChange));
                        return;
                    }
                    else
                    {
                        if (!GetAllowToTogglesTheSign(d) && e.TheTransferText == "+" && e.SelectionStart == 0)
                        {
                            int PreSS = e.SelectionStart;
                            e.TextAfterTheChange = e.TextAfterTheChange.Replace("+", "").Replace("-", "");
                            e.SelectionStart = PreSS - 1;
                            e.Cancel = true;
                            OnNotAllowedInput(new InputEventArgs(e.TheTransferText, e.TextAfterTheChange));
                            return;
                        }
                    }
                }
            }
        //
        finish:
            OnAllowedInput(new InputEventArgs(e.TheTransferText, e.TextAfterTheChange));
        }

        #endregion Private Methods

        #region Numeric

        static bool CheckToAddNumber(DependencyObject d, string TheTransferText, string TextAfterTheChange)
        {
            if (!(d is TextBox textBox)) return true;
            switch (GetNumericType(d))
            {
                case NumericTypes.DoNotAcceptDecimalSymbol:
                    if (TextAfterTheChange.Contains("."))
                    {
                        return false;
                    }
                    break;

                case NumericTypes.DoNotAcceptSign:
                    if (TextAfterTheChange.Contains("-") || TextAfterTheChange.Contains("+"))
                    {
                        return false;
                    }
                    break;

                case NumericTypes.DoNotAcceptBoth:
                    if (TextAfterTheChange.Contains(".") || TextAfterTheChange.Contains("-") || TextAfterTheChange.Contains("+"))
                    {
                        return false;
                    }
                    break;
                    //default:
                    //    break;
            }
            if (TextAfterTheChange.Contains(" ")) return false;
            if (TextAfterTheChange == ".") return true;
            if (SuccessToAddNumber(d, TextAfterTheChange)) return true;

            if (TheTransferText == ("-"))
            {
                if (TextAfterTheChange.IsNumeric()) return true;
                //if (AllowToCalculate) return true;
                //return !textBox.Text.StartsWith("-", StringComparison.Ordinal);
                //if (textBox.Text.Contains("-") == false)
                //{
                //    //_SetnewNumericVals(TheInputString)
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
            else if (TheTransferText == ("+"))
            {
                if (TextAfterTheChange.IsNumeric()) return true;
                //if (AllowToCalculate) return true;
                if (textBox.Text.Val() >= 0 && textBox.Text.Contains("-") == false)
                {
                    return false;
                }
                //else
                //{
                //    //_SetnewNumericVals(TheInputString)
                //    return true;
                //}
            }
            else if (TheTransferText == "." && textBox.Text == "")
            {
                //_SetnewNumericVals("0.")
                return true;
            }
            //else if (AllowToCalculate)
            //{
            //    return TextAfterTheChange.CanEvaluate();
            //}
            else
            {
                return false;
            }

            return false;
        }

        static bool SuccessToAddNumber(DependencyObject d, string TextAfterTheChange)
        {
            if (!TextAfterTheChange.IsNumeric()) return false;
            decimal Num = TextAfterTheChange.ValDecimal();
            return (Num >= GetMinNumber(d) && Num <= GetMaxNumber(d));
        }

        static void CheckZero(ref string string_1, ref int Pos_)
        {
            if (string_1 == "." || string_1 == "0.")
            {
                string_1 = "0.";
                Pos_ = 2;
            }
            else if (string_1.StartsWith(".", StringComparison.Ordinal))
            {
                string_1 = "0" + string_1;
                Pos_ += 1;
            }
            else if (string_1 == "-." || string_1 == "-0.")
            {
                string_1 = "-0.";
                Pos_ = 3;
            }
            else if (string_1.StartsWith("-.", StringComparison.Ordinal))
            {
                string_1 = "-0" + string_1.Replace("-", "");
                Pos_ += 2;
            }
            else if (string_1.ValDecimal() == 0)
            {
                if (string_1.StartsWith("-", StringComparison.Ordinal))
                {
                    if (string_1 == "-" | string_1 == "-0")
                    {
                        string_1 = "-0";
                    }
                    else
                    {
                        string_1 = "-" + string_1.Val().ToString();
                    }
                    Pos_ = string_1.Length;
                }
                else
                {
                    string_1 = "0";
                    Pos_ = 2;
                }
            }
            else if (string_1.StartsWith("0", StringComparison.Ordinal) && !string_1.StartsWith("0.", StringComparison.Ordinal))
            {
                string_1 = string_1.Remove(0, 1);
                if (string_1.Length == 1)
                {
                    Pos_ = 1;
                }
                else
                {
                    Pos_ -= 1;
                }
            }
            else if (string_1.StartsWith("-0", StringComparison.Ordinal) == true && !string_1.StartsWith("-0.", StringComparison.Ordinal))
            {
                string_1 = string_1.Remove(1, 1);
                if (string_1.Length == 1)
                {
                    Pos_ = 1;
                }
                else
                {
                    Pos_ -= 1;
                }
            }
        }

        static bool ContainsInvaildNumValues(DependencyObject d, string str, bool includeBrackets = false)
        {
            if (includeBrackets)
            {
                return AllowedChars(d).StringContainsUnlistedChars(str, Operators);
            }
            else
            {
                return AllowedChars(d).StringContainsUnlistedChars(str, OperatorsNBrackets());
            }
        }

        static bool IsAMathOperation(DependencyObject d)
        {
            if (!(d is TextBox textBox)) return false;
            return IsAMathOperation(textBox.Text);
        }

        public static bool IsAMathOperation(string str)
        {
            if (IsAnOperationOfTwoBrackets(str)) return true;
            return (!str.IsNumeric() && ContainsOperator(str));
        }

        static bool IsAnOperationOfTwoBrackets(DependencyObject d)
        {
            if (!(d is TextBox textBox)) return false;
            return IsAnOperationOfTwoBrackets(textBox.Text);
        }

        public static bool IsAnOperationOfTwoBrackets(string str)
        {
            return str.StartsWith("(", StringComparison.Ordinal) && str.EndsWith(")", StringComparison.Ordinal);
        }

        public static char[] Operators = { '-', '+', '/', '*', '%' };
        public static char[] Brackets = { '(', ')' };

        public static char[] OperatorsNBrackets()
        {
            char[] Res = new char[(Operators.Count() - 1) + (Brackets.Count() - 1)];
            Operators.CopyTo(Res, 0);
            Brackets.CopyTo(Res, 0);
            return Res;
        }

        public static bool IsOperator(string str)
        {
            return (str == "-" || str == "+" || str == "/" || str == "*"
                || str == "%");
        }

        public static bool ContainsOperator(string str)
        {
            return (str.Contains("-") || str.Contains("+") ||
                str.Contains("/") || str.Contains("*") || str.Contains("%"));
        }

        #endregion Numeric

        #region Handle text input/delete, OnKeyDown and cut/copy/paste commands

        static void PreviewExecutedHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            if (e.Command == ApplicationCommands.Delete)
            {
                e.Handled = true;
                RaiseTextChangingEvent_NCheck(textBox, EditOperations.Delete);
            }
            else if (e.Command == ApplicationCommands.Cut)
            {
                e.Handled = true;
                string SText = textBox.SelectedText;
                if (SText.IsNullOrEmpty())
                {
                    e.Handled = true;
                    //IsCut = false;
                    return;
                }
                var NewTC_C = GetTextChangingEventArgs(textBox, EditOperations.Cut);
                var TCut = NewTC_C.ToTextCutPasteEventArgs();
                //OnTextCut(TCut);
                if (!TCut.Cancel)
                {
                    NewTC_C.FromTextCutPasteEventArgs(TCut);
                    RaiseTextChangingEvent_NCheck(textBox, NewTC_C);
                    if (!NewTC_C.Cancel) Clipboard.SetText(TCut.TheTransferText);
                }
            }
            else if (e.Command == ApplicationCommands.Copy)
            {
                string SText = textBox.SelectedText;
                if (SText.IsNullOrEmpty())
                {
                    e.Handled = true;
                    //IsCut = false;
                    return;
                }
                var TCopyEA = new TextCopyEventArgs(SText);
                //OnTextCopy(TCopyEA);
                if (TCopyEA.Cancel)
                {
                    e.Handled = true;
                    return;
                }
                if (TCopyEA.TheTransferText != SText) //manual copy
                {
                    e.Handled = true;
                    Clipboard.SetText(TCopyEA.TheTransferText);
                }
            }
            else if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
                if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                {
                    string PText = Clipboard.GetText();
                    if (PText.IsNullOrEmpty()) return;
                    var NewTC_P = GetTextChangingEventArgs(textBox, EditOperations.Paste);
                    var TPaste = NewTC_P.ToTextCutPasteEventArgs();
                    //OnTextPaste(TPaste);
                    if (!TPaste.Cancel) // RaiseTextChangingEvent if paste Event not  Canceled
                    {
                        NewTC_P.FromTextCutPasteEventArgs(TPaste);
                        RaiseTextChangingEvent_NCheck(textBox, NewTC_P);
                    }
                }
            }
        }

        static void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            if (e.Text.Length == 1 && !char.IsControl(e.Text[0]))
            {
                string TransText = e.Text;
                TextChangingEventArgs NewTC = GetTextChangingEventArgs(textBox, EditOperations.CSelectedText, TransText);
                string TAfter = NewTC.TextAfterTheChange;
                CheckTextChanging(textBox, NewTC);
                bool _Handled = NewTC.Cancel;
                if (!NewTC.Cancel)
                {
                    OnTextChanging(NewTC);
                    _Handled = NewTC.Cancel;
                    if (!NewTC.Cancel)
                    {
                        if (NewTC.TheTransferText != TransText || TAfter != NewTC.TextAfterTheChange)
                        {
                            _Handled = true;
                            textBox.Text = NewTC.TextAfterTheChange;
                            textBox.SelectionStart = NewTC.SelectionStart;
                        }
                    }
                }
                e.Handled = _Handled;
            }
        }

        static void PreviewKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.A)
                {
                    e.Handled = true;
                    textBox.SelectAll();
                }
                else if (e.Key == Key.Back)
                {
                    TextChangingEventArgs NewTC = GetTextChangingEventArgs(textBox, EditOperations.CSelectedText, "\b");
                    string TAfter = NewTC.TextAfterTheChange;
                    CheckTextChanging(textBox, NewTC);
                    bool _Handled = NewTC.Cancel;
                    if (!NewTC.Cancel)
                    {
                        OnTextChanging(NewTC);
                        _Handled = NewTC.Cancel;
                        if (!NewTC.Cancel)
                        {
                            if (NewTC.TheTransferText != "\b" || TAfter != NewTC.TextAfterTheChange)
                            {
                                _Handled = true;
                                textBox.Text = NewTC.TextAfterTheChange;
                                textBox.SelectionStart = NewTC.SelectionStart;
                            }
                        }
                    }
                    e.Handled = _Handled;
                }
            }
            else if (e.Key == Key.Back)
            {
                //e.SuppressKeyPress = true;
                e.Handled = true;
                RaiseTextChangingEvent_NCheck(textBox, EditOperations.BackSpace);
            }
            else if (e.Key == Key.Delete)
            {
                //e.SuppressKeyPress = true;
                e.Handled = true;
                RaiseTextChangingEvent_NCheck(textBox, EditOperations.Delete);
            }
            else if ((e.Key == Key.Enter || e.Key == Key.Return)
                   && !((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                   && !((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                   && !((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
            {
                //e.SuppressKeyPress = true;
                e.Handled = true;
                SubmitCurrent(textBox);
            }
            else if (e.Key == Key.Space)
            {
                e.Handled = true;
                RaiseTextChangingEvent_NCheck(textBox, EditOperations.CSelectedText, " ");
            }
            //else
            //{
            //    var kChar = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            //    if (char.IsControl(kChar))
            //    {
            //        e.Handled = true;
            //        RaiseTextChangingEvent_NCheck(EditOperations.CSelectedText);
            //    }
            //}
            //
        }

        static bool SubmitCurrent(DependencyObject d)
        {
            if (!(d is TextBox textBox)) return true;
            if (GetEditType(d) == EditTypes.Numeric && IsAMathOperation(textBox.Text))
            {
                try
                {
                    string res = textBox.Text.EvaluateString();
                    if (res.IsNumeric())
                    {
                        if (CheckToAddNumber(d, res, res))
                        {
                            textBox.Text = res;
                            textBox.SelectionStart = textBox.Text.Length;
                            //if (RaiseSubmitAfterNumEvaluating) OnSubmit(EventArgs.Empty);
                            return true;
                        }
                        else
                        {
                            OnNotAllowedInput(new InputEventArgs(res, res));
                            //return false;
                        }
                    }
                    else
                    {
                        OnNotAllowedInput(new InputEventArgs(res, res));
                        //return false;
                    }
                }
                catch
                {
                    OnNotAllowedInput(new InputEventArgs(textBox.Text, "-Evaluating Error-"));
                    //return false;
                }
                return false;
            }
            else
            {
                //if (raiseEvent) OnSubmit(EventArgs.Empty);
                return true;
            }
        }

        #endregion Handle text input/delete, OnKeyDown and cut/copy/paste commands


        /*
        #region Public Methods

        /// <summary>
        /// To focus the control without selecting the text
        /// </summary>
        /// <param name="SelectionStart"></param>
        /// <param name="RemovedLength"></param>
        /// <remarks></remarks>
        static void FocusWithoutSelect(TextBox textBox, int? SelectionStart = null, int RemovedLength = 0)
        {
            int SStart = 0;
            if (SelectionStart.HasValue && SelectionStart.Value > 0)
            {
                SStart = SelectionStart.Value;
            }
            else
            {
                SStart = textBox.SelectionStart;
            }
            textBox.SelectionLength = 0;
            textBox.SelectionStart = SStart - RemovedLength;
            textBox.Focus();
        }

        /// <summary>
        /// Performs the Delete operation like Delete key.
        /// </summary>
        static void Delete()
        {
            //RaiseTextChangingEvent_NCheck(EditOperations.Delete);
        }

        /// <summary>
        /// Performs the BackSpace operation like BackSpace key.
        /// </summary>
        static void BackSpace()
        {
            //RaiseTextChangingEvent_NCheck(EditOperations.BackSpace);
        }

        public bool CanDelete
        {
            get
            {
                if (textBox.IsReadOnly) return false;
                if (textBox.SelectedText.IsNullOrEmpty())
                {
                    return (textBox.SelectionStart < textBox.Text.Length);
                }
                else
                {
                    return true;
                }
            }
        }

        public bool CanBackSpace
        {
            get
            {
                if (textBox.IsReadOnly) return false;
                if (textBox.SelectedText.IsNullOrEmpty())
                {
                    return (textBox.SelectionStart > 0);
                }
                else
                {
                    return true;
                }
            }
        }
        
            #endregion Public Methods
        */


        #endregion


        #region Events Handlers and Args

        public abstract class TextTransfer : EventArgs
        {

            /// <summary>
            ///  To cancel the operation.
            /// </summary>
            public bool Cancel { get; set; }

            /// <summary>
            /// Gets or Sets the transfer text.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string TheTransferText { get; set; }

            //
            /// <summary>
            /// Checks if The TransferText contains a control character
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool ContainsControlCharacter() => ContainsControlCharacter(TheTransferText);
            /// <summary>
            /// Checks if TextToCheckFor contains a control character
            /// </summary>
            /// <param name="TextToCheckFor"></param>
            /// <returns></returns>
            public static bool ContainsControlCharacter(string TextToCheckFor)
            {
                foreach (char Char_ in TextToCheckFor)
                {
                    if (char.IsControl(Char_)) return true;
                }
                return false;
            }

            /// <summary>
            /// Replace control characters in The TransferText
            /// </summary>
            /// <param name="Replacement"></param>
            /// <remarks></remarks>
            public void ReplaceControlCharacters(string Replacement = "")
            {
                List<char> ControlChars = new List<char>();
                foreach (char Char_ in TheTransferText)
                {
                    if (char.IsControl(Char_) == true)
                    {
                        ControlChars.Add(Char_);
                    }
                }
                foreach (char Char_ in ControlChars)
                {
                    TheTransferText = Regex.Replace(TheTransferText, Char_.ToString(), Replacement);
                }
            }
            /// <summary>
            /// Replace control characters in Text_
            /// </summary>
            /// <param name="Text_"></param>
            /// <param name="Replacement"></param>
            public static void ReplaceControlCharactersFromText(ref string Text_, string Replacement = "")
            {
                List<char> ControlChars = new List<char>();
                foreach (char Char_ in Text_)
                {
                    if (char.IsControl(Char_) == true)
                    {
                        ControlChars.Add(Char_);
                    }
                }
                foreach (char Char_ in ControlChars)
                {
                    Text_ = Regex.Replace(Text_, Char_.ToString(), Replacement);
                }
            }

        }

        //
        public delegate void TextCopyEventHandler(object sender, TextCopyEventArgs e);
        public class TextCopyEventArgs : TextTransfer
        {
            public TextCopyEventArgs(string TheTransferText_)
                : base()
            {
                TheTransferText = TheTransferText_;
            }
        }

        //
        public delegate void TextCutPasteEventHandler(object sender, TextCutPasteEventArgs e);
        public class TextCutPasteEventArgs : TextTransfer
        {
            public TextCutPasteEventArgs(TextBox CTextBox_, string TheTransferText_, string TextBeforeTheChange_, string TextAfterTheChange_, int SelectionStart_, CutPasteOperations OperationIs_)
                : base()
            {
                _CTextBox = CTextBox_;
                _TheTransferText = TheTransferText_;
                _TextBeforeTheChange = TextBeforeTheChange_;
                _TextAfterTheChange = TextAfterTheChange_;
                _SelectionStart = SelectionStart_;
                _OperationIs = OperationIs_;
            }


            private string _TheTransferText;
            /// <summary>
            /// Gets or Sets the transfer text.(it may change _TextAfterTheChange too)
            /// </summary>
            public new string TheTransferText
            {
                get { return _TheTransferText; }
                set { SetTheTransferText(value); }
            }
            private TextBox _CTextBox;
            private void SetTheTransferText(string theTransText)
            {
                //if (_CTextBox == null) _CTextBox = new TextBox();
                //_CTextBox.Text = _TextBeforeTheChange;
                //_CTextBox.SelectionStart = _TextBoxEzz.SelectionStart;
                //_CTextBox.SelectionLength = _TextBoxEzz.SelectionLength;
                //
                string TBefore = _CTextBox.Text;
                switch (OperationIs)
                {
                    case CutPasteOperations.Cut:
                        if (!_CTextBox.SelectedText.IsNullOrEmpty())
                        {
                            _CTextBox.SelectedText = string.Empty;
                        }
                        break;
                    case CutPasteOperations.Paste:
                        _CTextBox.SelectedText = theTransText;
                        break;
                }

                //
                _TheTransferText = theTransText;
                _TextAfterTheChange = _CTextBox.Text;
                _SelectionStart = _CTextBox.SelectionStart;
            }


            private string _TextBeforeTheChange;
            /// <summary>
            /// Gets the text before the change.
            /// </summary>
            public string TextBeforeTheChange { get { return _TextBeforeTheChange; } }


            private string _TextAfterTheChange;
            /// <summary>
            /// Gets or sets the text after the change. Note that: The SelectionStart will be rest if you changed it as the Text will be replaced with it.
            /// </summary>
            public string TextAfterTheChange
            {
                get { return _TextAfterTheChange; }
                set
                {
                    _TextAfterTheChange = value;
                    _SelectionStart = 0;
                }
            }


            private int _SelectionStart;
            /// <summary>
            /// Gets or sets the starting point of text selected in the control after the event . If the value is less than zero then it will be modified to zero(0).
            /// </summary>
            /// <value></value>
            /// <returns>The starting position of text selected in the control.</returns>
            /// <remarks></remarks>
            public int SelectionStart
            {
                get
                {
                    if (_SelectionStart < 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return _SelectionStart;
                    }
                }
                set
                {
                    if (value < 0)
                    {
                        _SelectionStart = 0;
                    }
                    else
                    {
                        _SelectionStart = value;
                    }
                    //textChangingMethod.NewSelectionStart_ = _NewSelectionStart;
                }
            }

            public CutPasteOperations CutOrPaste { get; set; }
            //
            public enum CutPasteOperations
            {
                Cut,
                Paste
            }
            private CutPasteOperations _OperationIs;
            public CutPasteOperations OperationIs
            {
                get { return _OperationIs; }
            }

            //
            public void FromTextChangingEventArgs(TextChangingEventArgs TCEv)
            {
                _TheTransferText = TCEv.TheTransferText;
                _TextBeforeTheChange = TCEv.TextBeforeTheChange;
                _TextAfterTheChange = TCEv.TextAfterTheChange;
                _OperationIs = TCEv.OperationIs == EditOperations.Cut ? CutPasteOperations.Cut : CutPasteOperations.Paste;
                _SelectionStart = TCEv.SelectionStart;
            }
            public TextChangingEventArgs ToTextChangingEventArgs()
            {
                TextChangingEventArgs newT = new TextChangingEventArgs(_CTextBox, _TheTransferText, _TextBeforeTheChange, _TextAfterTheChange, _SelectionStart, _OperationIs == CutPasteOperations.Cut ? EditOperations.Cut : EditOperations.Paste);
                newT.Cancel = this.Cancel;
                return newT;
            }
        }

        //
        public delegate void TextChangingEventHandler(object sender, TextChangingEventArgs e);
        public class TextChangingEventArgs : TextTransfer
        {

            public TextChangingEventArgs(TextBox CTextBox_, string TheTransferText_, string TextBeforeTheChange_, string TextAfterTheChange_, int SelectionStart_,
                                             EditOperations OperationsIs_)
                : base()
            {
                _CTextBox = CTextBox_;
                _TheTransferText = TheTransferText_;
                _TextBeforeTheChange = TextBeforeTheChange_;
                _TextAfterTheChange = TextAfterTheChange_;
                //
                _OperationIs = OperationsIs_;
                _SelectionStart = SelectionStart_;
            }


            private string _TheTransferText;
            /// <summary>
            /// Gets or Sets the transfer text.(it may change _TextAfterTheChange too)
            /// </summary>
            public new string TheTransferText
            {
                get { return _TheTransferText; }
                set { SetTheTransferText(value); }
            }
            private TextBox _CTextBox;
            private void SetTheTransferText(string theTransText)
            {
                //if (_CTextBox == null) _CTextBox = new TextBox();
                //_CTextBox.Text = _TextBeforeTheChange;
                //_CTextBox.SelectionStart = _TextBoxEzz.SelectionStart;
                //_CTextBox.SelectionLength = _TextBoxEzz.SelectionLength;
                //
                //string TBefore = _CTextBox.Text;
                switch (OperationIs)
                {
                    case EditOperations.CText:
                    case EditOperations.Other:
                        _CTextBox.Text = theTransText;
                        break;
                    case EditOperations.CSelectedText:
                        _CTextBox.SelectedText = theTransText;
                        break;
                    case EditOperations.BackSpace:
                    case EditOperations.Delete:
                        _TheTransferText = theTransText;
                        return;
                    //break;
                    case EditOperations.Cut:
                        if (!_CTextBox.SelectedText.IsNullOrEmpty())
                        {
                            _CTextBox.SelectedText = string.Empty;
                        }
                        break;
                    case EditOperations.Paste:
                        _CTextBox.SelectedText = theTransText;
                        break;
                }
                //
                _TheTransferText = theTransText;
                _TextAfterTheChange = _CTextBox.Text;
                _SelectionStart = _CTextBox.SelectionStart;
            }


            private string _TextBeforeTheChange;

            /// <summary>
            /// Gets the text before the change.
            /// </summary>
            public string TextBeforeTheChange { get { return _TextBeforeTheChange; } }

            private string _TextAfterTheChange;
            /// <summary>
            /// Gets or sets the text after the change. Note that: The SelectionStart will be rest if you changed it as the Text will be replaced with it.
            /// </summary>
            public string TextAfterTheChange
            {
                get { return _TextAfterTheChange; }
                set
                {
                    _TextAfterTheChange = value;
                    _SelectionStart = 0;
                    IsManuallyChanged = true;
                }
            }
            public bool IsManuallyChanged { get; private set; }

            private int _SelectionStart;
            /// <summary>
            /// Gets or sets the starting point of text selected in the control after the event . If the value is less than zero then it will be modified to zero(0).
            /// </summary>
            /// <value></value>
            /// <returns>The starting position of text selected in the control.</returns>
            /// <remarks></remarks>
            public int SelectionStart
            {
                get
                {
                    if (_SelectionStart < 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return _SelectionStart;
                    }
                }
                set
                {
                    if (value < 0)
                    {
                        _SelectionStart = 0;
                    }
                    else
                    {
                        _SelectionStart = value;
                    }
                    //textChangingMethod.NewSelectionStart_ = _NewSelectionStart;
                }
            }



            private EditOperations _OperationIs;
            public EditOperations OperationIs
            {
                get { return _OperationIs; }
            }

            public bool OperationIsInput() => (_OperationIs == EditOperations.CText || _OperationIs == EditOperations.CSelectedText
                            || _OperationIs == EditOperations.Paste || _OperationIs == EditOperations.Other);
            public bool OperationIsDelete() => (_OperationIs == EditOperations.Cut ||
                    _OperationIs == EditOperations.Delete ||
                    _OperationIs == EditOperations.BackSpace);
            //
            public void FromTextCutPasteEventArgs(TextCutPasteEventArgs TCPEv)
            {
                _TheTransferText = TCPEv.TheTransferText;
                _TextBeforeTheChange = TCPEv.TextBeforeTheChange;
                _TextAfterTheChange = TCPEv.TextAfterTheChange;
                _OperationIs = TCPEv.OperationIs == TextCutPasteEventArgs.CutPasteOperations.Cut ? EditOperations.Cut : EditOperations.Paste;
                _SelectionStart = TCPEv.SelectionStart;
            }
            public TextCutPasteEventArgs ToTextCutPasteEventArgs()
            {
                TextCutPasteEventArgs newT = new TextCutPasteEventArgs(_CTextBox, _TheTransferText, _TextBeforeTheChange, _TextAfterTheChange, _SelectionStart, _OperationIs == EditOperations.Cut ? TextCutPasteEventArgs.CutPasteOperations.Cut : TextCutPasteEventArgs.CutPasteOperations.Paste);
                newT.Cancel = this.Cancel;
                return newT;
            }
        }

        //
        public delegate void InputEventHandler(object sender, InputEventArgs e);
        public class InputEventArgs : EventArgs
        {
            public InputEventArgs(string TheTransferText_, string TextAfterTheChange_)
            {
                _TheTransferText = TheTransferText_;
                _TextAfterTheChange = TextAfterTheChange_;
            }
            string _TheTransferText;
            public string TheTransferText
            {
                get { return _TheTransferText; }
            }
            string _TextAfterTheChange;
            public string TextAfterTheChange
            {
                get { return _TextAfterTheChange; }
            }
        }

        ////Submit
        //public delegate void SubmitEventHandler(object sender, SubmitEventArgs e);
        //public class SubmitEventArgs : EventArgs
        //{
        //    public SubmitEventArgs(string TextBeforeTheChange_, string TextAfterTheChange_)
        //    {
        //        _TextBeforeTheChange = TextBeforeTheChange_;
        //        _TextAfterTheChange = TextAfterTheChange_;
        //    }
        //    string _TextBeforeTheChange;
        //    public string TextBeforeTheChange
        //    {
        //        get { return _TextBeforeTheChange; }
        //    }
        //    string _TextAfterTheChange;
        //    public string TextAfterTheChange
        //    {
        //        get { return _TextAfterTheChange; }
        //    }
        //}

        #endregion  //Events Handlers and Args

        #endregion //EzzTextBox

    }
}
