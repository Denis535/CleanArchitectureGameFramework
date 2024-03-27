#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class VisualElementWrapper<T> : IVisualElementWrapper<T> where T : notnull, VisualElement {

        protected T VisualElement { get; }
        T IVisualElementWrapper<T>.VisualElement => VisualElement;

        public VisualElementWrapper(T visualElement) {
            VisualElement = visualElement;
        }

    }
    // Element
    public class ElementWrapper : VisualElementWrapper<VisualElement> {

        public ElementWrapper(VisualElement visualElement) : base( visualElement ) {
        }

    }
    // Label
    public class LabelWrapper : VisualElementWrapper<Label> {

        public string? Text {
            get => VisualElement.text;
            set => VisualElement.text = value;
        }

        public LabelWrapper(Label visualElement) : base( visualElement ) {
        }

    }
    // Button
    public class ButtonWrapper : VisualElementWrapper<Button> {

        public string? Text {
            get => VisualElement.text;
            set => VisualElement.text = value;
        }

        public ButtonWrapper(Button visualElement) : base( visualElement ) {
        }

    }
    public class RepeatButtonWrapper : VisualElementWrapper<RepeatButton> {

        public string? Text {
            get => VisualElement.text;
            set => VisualElement.text = value;
        }

        public RepeatButtonWrapper(RepeatButton visualElement) : base( visualElement ) {
        }

    }
    // Image
    public class ImageWrapper : VisualElementWrapper<Image> {

        public Texture Image {
            get => VisualElement.image;
            set => VisualElement.image = value;
        }
        public Sprite Sprite {
            get => VisualElement.sprite;
            set => VisualElement.sprite = value;
        }
        public VectorImage VectorImage {
            get => VisualElement.vectorImage;
            set => VisualElement.vectorImage = value;
        }
        public Color Color {
            get => VisualElement.tintColor;
            set => VisualElement.tintColor = value;
        }
        public ScaleMode ScaleMode {
            get => VisualElement.scaleMode;
            set => VisualElement.scaleMode = value;
        }
        public Rect Rect {
            get => VisualElement.sourceRect;
            set => VisualElement.sourceRect = value;
        }
        public Rect RectUV {
            get => VisualElement.uv;
            set => VisualElement.uv = value;
        }

        public ImageWrapper(Image visualElement) : base( visualElement ) {
        }

    }
    // TextField
    public class TextFieldWrapper<T> : VisualElementWrapper<BaseField<string?>> where T : notnull {

        public string? Label {
            get => VisualElement.label;
            set => VisualElement.label = value;
        }
        public string? Value {
            get => VisualElement.value;
            set => VisualElement.value = value;
        }

        public TextFieldWrapper(BaseField<string?> visualElement) : base( visualElement ) {
            Assert.Operation.Message( $"TextWrapper {this} is invalid" ).Valid( this is TextFieldWrapper<string> );
        }

    }
    // PopupField
    public class PopupFieldWrapper<T> : VisualElementWrapper<PopupField<T?>> where T : notnull {

        public string? Label {
            get => VisualElement.label;
            set => VisualElement.label = value;
        }
        public T? Value {
            get => VisualElement.value;
            set => VisualElement.value = value;
        }
        public T?[] Choices {
            get => VisualElement.choices.ToArray();
            set => VisualElement.choices = value.ToList();
        }
        public (T? Value, T?[] Choices) ValueChoices {
            get => (VisualElement.value, VisualElement.choices.ToArray());
            set => (VisualElement.value, VisualElement.choices) = (value.Value, value.Choices.ToList());
        }

        public PopupFieldWrapper(PopupField<T?> visualElement) : base( visualElement ) {
        }

    }
    // SliderField
    public class SliderFieldWrapper<T> : VisualElementWrapper<BaseSlider<T>> where T : struct, IComparable<T> {

        public string? Label {
            get => VisualElement.label;
            set => VisualElement.label = value;
        }
        public T Value {
            get => VisualElement.value;
            set => VisualElement.value = value;
        }
        public T Min {
            get => VisualElement.lowValue;
            set => VisualElement.lowValue = value;
        }
        public T Max {
            get => VisualElement.highValue;
            set => VisualElement.highValue = value;
        }
        public (T Value, T Min, T Max) ValueMinMax {
            get => (VisualElement.value, VisualElement.lowValue, VisualElement.highValue);
            set => (VisualElement.value, VisualElement.lowValue, VisualElement.highValue) = (value.Value, value.Min, value.Max);
        }

        public SliderFieldWrapper(BaseSlider<T> visualElement) : base( visualElement ) {
        }

    }
    // ToggleField
    public class ToggleFieldWrapper<T> : VisualElementWrapper<Toggle> where T : struct, IComparable<T> {

        public string? Label {
            get => VisualElement.label;
            set => VisualElement.label = value;
        }
        public bool Value {
            get => VisualElement.value;
            set => VisualElement.value = value;
        }

        public ToggleFieldWrapper(Toggle visualElement) : base( visualElement ) {
            Assert.Operation.Message( $"ToggleWrapper {this} is invalid" ).Valid( this is ToggleFieldWrapper<bool> );
        }

    }
    // Slot
    public class WidgetSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIWidgetBase {

        public T? Widget { get; private set; }

        public WidgetSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Add(T widget) {
            Assert.Operation.Message( $"Slot must have no widget" ).Valid( Widget == null );
            Widget = widget;
            VisualElement.Add( widget );
        }
        public void Remove(T widget) {
            Assert.Operation.Message( $"Slot must have widget" ).Valid( Widget != null );
            Assert.Operation.Message( $"Slot must have {widget} widget" ).Valid( Widget == widget );
            Widget = null;
            VisualElement.Remove( widget );
        }

    }
    public class ViewSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIViewBase {

        public T? View { get; private set; }

        public ViewSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Add(T view) {
            Assert.Operation.Message( $"Slot must have no view" ).Valid( View == null );
            View = view;
            VisualElement.Add( view );
        }
        public void Remove(T view) {
            Assert.Operation.Message( $"Slot must have view" ).Valid( View != null );
            Assert.Operation.Message( $"Slot must have {view} view" ).Valid( View == view );
            View = null;
            VisualElement.Remove( view );
        }

    }
    // ListSlot
    public class WidgetListSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIWidgetBase {

        private List<T> Widgets_ { get; } = new List<T>();
        public IReadOnlyList<T> Widgets => Widgets_;

        public WidgetListSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Add(T widget) {
            Assert.Operation.Message( $"Slot must have no {widget} widget" ).Valid( !Widgets_.Contains( widget ) );
            Widgets_.Add( widget );
            VisualElement.Add( widget );
        }
        public void Remove(T widget) {
            Assert.Operation.Message( $"Slot must have widget" ).Valid( Widgets_.Any() );
            Assert.Operation.Message( $"Slot must have {widget} widget" ).Valid( Widgets_.Contains( widget ) );
            Widgets_.Remove( widget );
            VisualElement.Remove( widget );
        }
        public void Clear() {
            foreach (var widget in Widgets_) {
                VisualElement.Remove( widget );
            }
            Widgets_.Clear();
        }

    }
    public class ViewListSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIViewBase {

        private List<T> Views_ { get; } = new List<T>();
        public IReadOnlyList<T> Views => Views_;

        public ViewListSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Add(T view) {
            Assert.Operation.Message( $"Slot must have no {view} view" ).Valid( !Views_.Contains( view ) );
            Views_.Add( view );
            VisualElement.Add( view );
        }
        public void Remove(T view) {
            Assert.Operation.Message( $"Slot must have view" ).Valid( Views_.Any() );
            Assert.Operation.Message( $"Slot must have {view} view" ).Valid( Views_.Contains( view ) );
            Views_.Remove( view );
            VisualElement.Remove( view );
        }
        public void Clear() {
            foreach (var view in Views_) {
                VisualElement.Remove( view );
            }
            Views_.Clear();
        }

    }
    // StackSlot
    public class WidgetStackSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIWidgetBase {

        private Stack<T> Widgets_ { get; } = new Stack<T>();
        public IReadOnlyCollection<T> Widgets => Widgets_;

        public WidgetStackSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Push(T widget) {
            Assert.Operation.Message( $"Slot must have no {widget} widget" ).Valid( !Widgets_.Contains( widget ) );
            if (Widgets_.TryPeek( out var last )) {
                VisualElement.Remove( last );
            }
            {
                Widgets_.Push( widget );
                VisualElement.Add( widget );
            }
        }
        public T Peek() {
            Assert.Operation.Message( $"Slot must have widget" ).Valid( Widgets_.Any() );
            var result = Widgets_.Peek();
            return result;
        }
        public T Pop() {
            Assert.Operation.Message( $"Slot must have widget" ).Valid( Widgets_.Any() );
            var result = default( T );
            {
                result = Widgets_.Pop();
                VisualElement.Remove( result );
            }
            if (Widgets_.TryPeek( out var last )) {
                VisualElement.Add( last );
            }
            return result;
        }

    }
    public class ViewStackSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIViewBase {

        private Stack<T> Views_ { get; } = new Stack<T>();
        public IReadOnlyCollection<T> Views => Views_;

        public ViewStackSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Push(T view) {
            Assert.Operation.Message( $"Slot must have no {view} view" ).Valid( !Views_.Contains( view ) );
            if (Views_.TryPeek( out var last )) {
                VisualElement.Remove( last );
            }
            {
                Views_.Push( view );
                VisualElement.Add( view );
            }
        }
        public T Peek() {
            Assert.Operation.Message( $"Slot must have view" ).Valid( Views_.Any() );
            var result = Views_.Peek();
            return result;
        }
        public T Pop() {
            Assert.Operation.Message( $"Slot must have view" ).Valid( Views_.Any() );
            var result = default( T );
            {
                result = Views_.Pop();
                VisualElement.Remove( result );
            }
            if (Views_.TryPeek( out var last )) {
                VisualElement.Add( last );
            }
            return result;
        }

    }
}
