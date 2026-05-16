# Home

**DarkUI** is a dark themed control and docking library for .NET WinForms. Contained within the library are dozens of components which can be combined to create simple, clean, and performant user interfaces in the style of popular tabbed document interfaces such as Visual Studio, Photoshop, WebStorm, and XCode.

### Getting started

* [Using DarkUI](#using-darkui) - Learn how to add DarkUI to your project.

* [Modern control scrolling](#modern-control-scrolling) - Enable intuitive control scrolling.

* [Forms and dialogs](#forms-and-dialogs) - Create forms and dialogs.

* [Responsive layouts](#responsive-layouts) - Develop responsive control layouts.

* [Tabbed document interface](#tabbed-document-interface) - Design a Visual Studio style interface.

## Forms and dialogs

> [Home](#home) ▸ **Forms and dialogs**

`DarkForm` inherits from `Form` so can be a drop-in replacement for any references you have to this within your project.

If you modify the blank `Form1` class created with a fresh WinForms project you can instantly get started with a dark-themed form.

```csharp
public partial class Form1 : DarkForm
{
    public Form1()
    {
        InitializeComponent();
    }
}
```

You should immediately see your changes in the designer.

<img src="Resources\1.png">

### Creating a dialog

`DarkDialog` is a `DarkForm` with additional functionality which simplifies creating dialog forms.

As before with the `DarkForm` example, simply change any instance of `Form` to `DarkDialog` to use this class.

```csharp
public partial class Form1 : DarkDialog
{
    public Form1()
    {
        InitializeComponent();
    }
}
```

It'll look very similar to a `DarkForm` except you'll now have an 'Ok' button at the bottom.

<img src="Resources\2.png">

This is the dialog footer, and can be changed to show a different combination of standard dialog action buttons.

Currently, the following combinations are possible.

* Ok
* Close
* OkCancel
* YesNo
* YesNoCancel
* AbortRetryIgnore
* RetryCancel

You can define which buttons are shown by the dialog by modifying the `DialogButtons` property.

<img src="Resources\3.png">

Each of the buttons are exposed to any subclasses which inherit from the `DarkDialog` super class, so you can modify their properties and hook in to the events as you would with any other object.

```csharp
btnOk.Text = "Accept";
btnOk.Click += BtnOk_Click;
```

The buttons will automatically align themselves and keep everything perfectly spaced so you shouldn't have to worry about anything other than that.

### Using message boxes
DarkUI offers 3 types of message boxes.

Information box.

<img src="Resources\4.png">

Warning box.

<img src="Resources\5.png">

Error box.

<img src="Resources\6.png">

To call each of these use the `DarkMessageBox` class. You can either create an object manually, or call the static methods available.

```csharp
DarkMessageBox.ShowInformation("This is an information box", "Dark UI Example");
DarkMessageBox.ShowWarning("This is a warning box", "Dark UI Example");
DarkMessageBox.ShowError("This is an error box", "Dark UI Example");
```

`DarkMessageBox` is a subclass of `DarkDialog` so you can modify which buttons are shown in the same way.

<img src="Resources\7.png">

The message box is designed to properly size and position itself based on the content you pass in. As such, very long text will still look very presentable (unlike default WinForms).

<img src="Resources\8.png">

## Modern control scrolling

> [Home](#home) ▸ **Modern control scrolling**

In Windows Vista the guidelines for mouse wheel control scrolling changed. Previously using the mouse wheel would scroll the control which has focus at the time, regardless of where the actual mouse cursor is. Modern applications instead send the mouse wheel events to the control which the cursor is currently hovering over instead.

To emulate this behaviour in .NET WinForms there are various solutions available, with the best one probably being the usage of the `IMessageFilter` interface.

### ControlScrollFilter class

DarkUI contains an instance of this implementation which will intercept all incoming mouse wheel events and re-route them to the control you're currently hovering over instead of the focused control.

This functionality is encapsulated in the `ControlScrollFilter` class.

```csharp
public class ControlScrollFilter : IMessageFilter
{
    public bool PreFilterMessage(ref Message m)
    {
        switch (m.Msg)
        {
            case (int)WM.MOUSEWHEEL:
            case (int)WM.MOUSEHWHEEL:
                var hControlUnderMouse = Native.WindowFromPoint(new Point((int)m.LParam));

                if (hControlUnderMouse == m.HWnd)
                    return false;

                Native.SendMessage(hControlUnderMouse, (uint)m.Msg, m.WParam, m.LParam);
                return true;
        }

        return false;
    }
}
```

To enable this functionality within your application, simply add the following line somewhere which is only going to be called once (preferably before the user is able to interact with any controls).

```csharp
Application.AddMessageFilter(new ControlScrollFilter());
```

Personally I place this within the `Program` class just before calling `Application.Run()`

## Responsive layouts

> [Home](#home) ▸ **Responsive layouts**

Creating high quality layouts in WinForms can be a bit of a chore. Personally I make use of the `Dock` property to make sure my controls properly fill and conform to the container they're presented in.

The controls in DarkUI were designed to be used in much the same way.

### Form and dialog spacing

To properly dock controls within a form we first need to define some whitespace which properly separates the content from the parent container.

First, we'll start off with a blank form or dialog.

<img src="Resources\2.png">

Then we add in a `Panel` control and set the `Dock` property to `Fill`. If we're using a blank form, we'll give this a `Padding` of `10, 10, 10, 10`. If it's a dialog, the footer already includes 10 pixels of padding, so we'll set the padding to `10, 10, 10, 0`.

With this panel in place, we can now add in some controls. Personally I like using the `DarkSectionPanel` control to properly define an area within the form.

<img src="Resources\9.png">

If we then add in a `DarkListView` with a `Dock` value of `Fill` and a `DarkToolStrip` with a `Dock` value of `Bottom` we'll be able to get a simple, responsive control layout which will automatically size itself based on the size of the container form.

<img src="Resources\10.png">

### Section spacing

Unfortunately the WinForms `Margin` property doesn't work quite like you'd expect if you come in from a web-development background. There are some controls you can use to make this easier, such as the `TableLayoutPanel`, however for spacing out individual panels I prefer to keep controls separate.

Let's modify our previous example and bring up a situation which requires more in-depth layout work.

Take the parent `Panel` you created and set the `Dock` property to `Left`, then re-size the parent form to open up space for a second section.

<img src="Resources\11.png">

We want to modify the panel on the left to contain three `DarkSectionPanel` controls instead of just one. To do this, we'll first add some additional `Panel` controls to dictate the vertical margins. 

First, move the existing `DarkSectionPanel` out of the left section panel so we can properly work in that control.

Second, add three new `Panel` controls. Set the `Dock` property of the top two panels to be `Top`. Set the third panel's `Dock` property to be `Fill`. This will ensure that the sum of all the panels will match the vertical size of the parent form.

<img src="Resources\12.png">

Now take our previous `DarkSectionPanel` and place it within the bottom-most panel and add in additional `DarkSectionPanel` controls to the top two panels. Make sure all of these have their `Dock` property set to `Fill`.

Also set the top two parent panels to have 10 pixels of bottom padding. This will make sure there is adequate spacing between the visible section panels.

<img src="Resources\13.png">)

We now have a pretty nice layout for the left column of our layout.

To finish up the rest, add a `Panel` control in the main form with the same padding value of your left column panel. Add a final `DarkSectionPanel` and set the `Dock` property to be `Fill`.

To make sure there's not too much padding between the left and right sections change their padding properties so the `Right` and `Left` values are 5 instead of 10. This will ensure the total padding between them matches the other areas.

<img src="Resources\14.png">

### Control spacing

So now we have some sections which properly conform to the size of the form. We've also seen how to create basic control layouts using the `Dock` property.

However, what if we want to create a vertical control flow which includes margins between each row? We use the `TableLayoutPanel` control.

As the `DarkSectionPanel` control doesn't support padding, you'll need to create a new `Panel` control which has 10 pixels of padding instead. Drag that in to the main `DarkSectionPanel` control, set the `Dock` property to `Fill` and then add in a `TableLayoutPanel`.

<img src="Resources\15.png">

Whilst the control has focus, clicking the arrow in the top right of the `TableLayoutPanel` will open up a menu. Selecting `Edit rows and columns...` will open up a dialog which allows you to customise the layout.

<img src="Resources\16.png">

As we'll only be using a single column for this example, you can delete the second one. Setting the first column to `AutoSize` will cause it to automatically fill the panel.

Switching to the row view, we'll now create as many rows as we think we'll need. We can always add more later. Setting each of these to have a size type of `AutoSize` will cause them to automatically wrap around their contents.

Accept your changes and your `TableLayoutPanel` is now ready to accept your controls. Drag in some labels and textboxes and you'll be able to create a simple vertical layout flow. 

As you do so, make sure their `Row` and `Column` properties are set properly, as the designer is a bit temperamental about these. Also, make sure to remove the `Margin` from each of them to stop the default values from screwing up your flow.

<img src="Resources\17.png">

It looks pretty terrible, doesn't it. The `TableLayoutPanel` actually respects the `Margin` properties of its child controls, so we'll now be able to use that to properly space out our controls.

As a rule of thumb I use a 10 pixel margin at the top and bottom of a section, then 5 pixel margins between controls *within* a section.

<img src="Resources\18.png">

So there we have it. A dynamic control flow which maintains consistent spacing between controls and properly sizes based on the parent container.

You can get a lot more clever with the layouts. Here's an example of one of the control layouts from my own application.

<img src="Resources\19.png">

Remember - spacing is everything.

## Tabbed document interface

> [Home](#home) ▸ **Tabbed document interface**

`DarkDockPanel` is the control you use to create a tabbed document interface. The dock panel itself contains 4 `DarkDockRegion` controls - Left, Right, Bottom, and Document. Each of these regions can be assigned `DarkDockContent` which will then be further divided in to a `DarkDockGroup` control which allows for tabbing between visible content.

Ultimately the majority of this functionality occurs under the hood. All you really need to care about is creating the `DarkDockPanel` on the form you want to have your tabbed interface, and then creating the various documents and tool windows which it will contain.

### Creating a DarkDockPanel

The `DarkDockPanel` control is easy enough to add. It'll appear in the toolbox with all the other DarkUI components and can be simply dragged on to a form.

<img src="Resources\20.png">

It won't look like much, but set the `Dock` property to `Fill` and we'll go over how to create content for it.

### Initialising message filters

One of the core concepts for the dock panel control is having single-pixel borders between components whilst allowing users to easily re-size content without struggling to grasp the splitters.

To achieve this the docking library uses an interface called `IMessageFilter` to pre-filter all incoming windows events and checks for splitter interaction before letting the events bubble down.

Enabling this functionality is fairly simple. The `DarkDockPanel` exposes two message filters: `DockContentDragFilter` and `DockResizeFilter`. The former deals with drag & drop functionality for tool windows whilst the latter deals with the resize splitters for regions.

Adding these to your application is done like this.

```csharp
Application.AddMessageFilter(DockPanel.DockContentDragFilter);
Application.AddMessageFilter(DockPanel.DockResizeFilter);
```

With the `DockContentDragFilter` in place you'll be able to click & drag the tool window headers within your dock panel and drag them around to other dock groups and regions.

<img src="Resources\21.png">

With the `DockResizeFilter` in place your single-pixel borders will actually act like they're 5 pixels wide, making them much easier for users to interact with. Dragging these splitters will also give you a nice semi-transparent guideline.

<img src="Resources\22.png">

### Creating DarkDockContent

The super class for all dock content is `DarkDockContent`. This is built on top of the `UserControl` class and can be modified in much the same way. As such, you can fully design your tabbed content using the visual designer.

DarkUI also exposes two specialised types of dock content called `DarkDocument` and `DarkToolWindow`. Documents are limited to the Document region of the dock panel and tool windows are limited to the Left, Right, and Bottom regions. `DarkToolWindow` objects have the additional ability to be dragged between dock groups and regions.

To inherit from these classes simply create a blank `UserControl` and change the super class.

```csharp
public partial class ToolWindow : DarkToolWindow
{
    public ToolWindow()
    {
        InitializeComponent();
    }
}
```

This will leave you with a blank tool window with the header visible in the designer.

<img src="Resources\23.png">

You can change the header/tab text with the `DockText` property. You can define which dock region the content will be added to by modifying the `DefaultDockArea` property. You can also set an image in the header with the `Icon` property.

Adding a `DarkTreeView` and `DarkToolStrip` control allows us to create a simple project explorer window.

<img src="Resources\24.png">

Placing this content within the dock panel is incredibly simple.

```csharp
DockPanel.AddContent(new ToolWindow());
```

If we run the application we'll see a very sparse dock panel.

<img src="Resources\25.png">

However, once we add more tool windows across multiple regions and load in some `DockDocument` controls, we have a pretty feature complete tabbed document interface.

<img src="Resources\26.png">

## Using DarkUI

> [Home](#home) ▸ **Using DarkUI**

To begin using DarkUI within a WinForms project simply reference it within Visual Studio.

<img src="Resources\27.png">

DarkUI consists of the following namespaces. Reference these within the files you'd like to use the various controls and components.

```csharp
using DarkUI.Collections;
using DarkUI.Config;
using DarkUI.Controls;
using DarkUI.Docking;
using DarkUI.Forms;
using DarkUI.Renderers;
```

### Using DarkUI controls

Once the DarkUI library is referenced your toolbox should have a section called 'DarkUI Components'. Within this section you'll find all of the controls and components the library offers.

<img src="Resources\28.png">

If you don't see this section you might need to select them manually. To do this right-click anywhere within the Tools window and select 'Choose Items...'

<img src="Resources\29.png">

On the 'Choose Toolbox Items' form click the 'Browse...' button and select the 'DarkUI.dll' file.

<img src="Resources\30.png">

This should add all of the DarkUI components to the list view. Accept these changes to make them show up in the toolbox.

<img src="Resources\31.png">

As with any WinForms control library just drag these on to a form or user control and you'll be able to use the visual designer to modify the look and feel of your application.
