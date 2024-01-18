#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Framework.UI;
    using UnityEngine.UIElements;

    public abstract class VisualElementWrapper {

        public VisualElement VisualElement { get; }

        public bool IsEnabled {
            get => VisualElement.enabledSelf;
            set => VisualElement.SetEnabled( value );
        }
        public bool IsDisplayed {
            get => VisualElement.IsDisplayed();
            set => VisualElement.SetDisplayed( value );
        }
        public bool IsValid {
            get => VisualElement.IsValid();
            set => VisualElement.SetValid( value );
        }

        public IReadOnlyList<string> Classes {
            get => (IReadOnlyList<string>) VisualElement.GetClasses();
        }

        public VisualElementWrapper(VisualElement visualElement) {
            VisualElement = visualElement;
        }

        public void AddClass(string @class) {
            VisualElement.AddToClassList( @class );
        }
        public void RemoveClass(string @class) {
            VisualElement.RemoveFromClassList( @class );
        }
        public void ToggleClass(string @class) {
            VisualElement.ToggleInClassList( @class );
        }
        public void EnableClass(string @class, bool isEnabled) {
            VisualElement.EnableInClassList( @class, isEnabled );
        }
        public bool ContainsClass(string @class) {
            return VisualElement.ClassListContains( @class );
        }
        public void ClearClasses() {
            VisualElement.ClearClassList();
        }

    }
    public abstract class VisualElementWrapper<T> : VisualElementWrapper where T : VisualElement {

        public new T VisualElement => (T) base.VisualElement;

        public VisualElementWrapper(T visualElement) : base( visualElement ) {
        }

    }
    public static class VisualElementWrapperExtensions {

        // Wrap
        public static ElementWrapper Wrap(this VisualElement visualElement) {
            return new ElementWrapper( visualElement );
        }
        public static LabelWrapper Wrap(this Label visualElement) {
            return new LabelWrapper( visualElement );
        }
        public static ButtonWrapper Wrap(this Button visualElement) {
            return new ButtonWrapper( visualElement );
        }
        public static RepeatButtonWrapper Wrap(this RepeatButton visualElement) {
            return new RepeatButtonWrapper( visualElement );
        }
        public static ImageWrapper Wrap(this Image visualElement) {
            return new ImageWrapper( visualElement );
        }
        public static TextFieldWrapper<string> Wrap(this BaseField<string?> visualElement) {
            return new TextFieldWrapper<string>( visualElement );
        }
        public static PopupFieldWrapper<T> Wrap<T>(this PopupField<T?> visualElement) where T : notnull {
            return new PopupFieldWrapper<T>( visualElement );
        }
        public static SliderFieldWrapper<T> Wrap<T>(this BaseSlider<T> visualElement) where T : struct, IComparable<T> {
            return new SliderFieldWrapper<T>( visualElement );
        }
        public static ToggleFieldWrapper<bool> Wrap(this Toggle visualElement) {
            return new ToggleFieldWrapper<bool>( visualElement );
        }

        // AsSlot
        public static SlotWrapper AsSlot(this VisualElement visualElement) {
            return new SlotWrapper( visualElement );
        }

        // OnAttachToPanel
        public static void OnAttachToPanel<T>(this VisualElementWrapper<T> wrapper, Action<VisualElementWrapper>? callback) where T : VisualElement {
            wrapper.VisualElement.OnAttachToPanel( evt => callback?.Invoke( wrapper ) );
        }
        public static void OnDetachFromPanel<T>(this VisualElementWrapper<T> wrapper, Action<VisualElementWrapper>? callback) where T : VisualElement {
            wrapper.VisualElement.OnDetachFromPanel( evt => callback?.Invoke( wrapper ) );
        }

        // OnClick
        public static void OnClick(this ButtonWrapper wrapper, Action<VisualElementWrapper>? callback) {
            wrapper.VisualElement.OnClick( evt => callback?.Invoke( wrapper ) );
        }

        // OnChange
        public static void OnChange(this TextFieldWrapper<string> wrapper, Action<VisualElementWrapper, string?>? callback) {
            wrapper.VisualElement.OnChange( evt => callback?.Invoke( wrapper, evt.newValue ) );
        }
        public static void OnChange(this TextFieldWrapper<string> wrapper, Action<VisualElementWrapper, string?, string?>? callback) {
            wrapper.VisualElement.OnChange( evt => callback?.Invoke( wrapper, evt.newValue, evt.previousValue ) );
        }

        // OnChange
        public static void OnChange<T>(this PopupFieldWrapper<T> wrapper, Action<VisualElementWrapper, T?>? callback) where T : notnull {
            wrapper.VisualElement.OnChange( evt => callback?.Invoke( wrapper, evt.newValue ) );
        }
        public static void OnChange<T>(this PopupFieldWrapper<T> wrapper, Action<VisualElementWrapper, T?, T?>? callback) where T : notnull {
            wrapper.VisualElement.OnChange( evt => callback?.Invoke( wrapper, evt.newValue, evt.previousValue ) );
        }

        // OnChange
        public static void OnChange<T>(this SliderFieldWrapper<T> wrapper, Action<VisualElementWrapper, T>? callback) where T : struct, IComparable<T> {
            wrapper.VisualElement.OnChange( evt => callback?.Invoke( wrapper, evt.newValue ) );
        }
        public static void OnChange<T>(this SliderFieldWrapper<T> wrapper, Action<VisualElementWrapper, T, T>? callback) where T : struct, IComparable<T> {
            wrapper.VisualElement.OnChange( evt => callback?.Invoke( wrapper, evt.newValue, evt.previousValue ) );
        }

        // OnChange
        public static void OnChange(this ToggleFieldWrapper<bool> wrapper, Action<VisualElementWrapper, bool>? callback) {
            wrapper.VisualElement.OnChange( evt => callback?.Invoke( wrapper, evt.newValue ) );
        }
        public static void OnChange(this ToggleFieldWrapper<bool> wrapper, Action<VisualElementWrapper, bool, bool>? callback) {
            wrapper.VisualElement.OnChange( evt => callback?.Invoke( wrapper, evt.newValue, evt.previousValue ) );
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
            Assert.Object.Message( $"TextWrapper {this} is invalid" ).Valid( this is TextFieldWrapper<string> );
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
            Assert.Object.Message( $"ToggleWrapper {this} is invalid" ).Valid( this is ToggleFieldWrapper<bool> );
        }

    }
    // Slot
    public class SlotWrapper : VisualElementWrapper<VisualElement> {

        public IReadOnlyList<VisualElement> Children {
            get => (IReadOnlyList<VisualElement>) VisualElement.Children();
        }

        public SlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Add(VisualElement element) {
            VisualElement.Add( element );
        }
        public void Remove(VisualElement element) {
            VisualElement.Remove( element );
        }
        public bool Contains(VisualElement element) {
            return VisualElement.Contains( element );
        }

        public void Add(UIViewBase view) {
            VisualElement.Add( view.VisualElement );
        }
        public void Remove(UIViewBase view) {
            VisualElement.Remove( view.VisualElement );
        }
        public bool Contains(UIViewBase view) {
            return VisualElement.Contains( view.VisualElement );
        }

    }
}
