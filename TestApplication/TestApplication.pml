Window {
  Title: "Perspex Test Application"

  Grid {
    ColumnDefinitions: "*,*"
    RowDefinitions: "1*,Auto"

    TabControls {
      Grid.ColumnSpan: 2

      TabItem {
        Header: "Buttons"
        StackPanel {
          Orientation: Vertical
          HorizontalAlignment: Center
          VerticalAlignment: Center
          Gap: 8
          MinWidth: 120

          Button {
            Content: "Button"
          }
          Button {
            Content: "Button"
            Background: 0xcc119eda
          }
          Button {
            Content: "Disabled"
            IsEnabled: false
          }
          Button {
            Content: "Disabled"
            IsEnabled: false
            Background: 0xcc119eda
          }
          ToggleButton {
              Content: "Toggle"
          }
          ToggleButton {
              Content: "Disabled"
              IsEnabled: false
          }
          CheckBox {
              Content: "Checkbox"
          }
          RadioButton
          {
              Content: "RadioButton 1"
              IsChecked: true
          },
          RadioButton
          {
              Content: "RadioButton 2"
          },
        }
      }

      TextBlock {
        Id: "fps"
        HorizontalAlignment: Left
        Grid.Row: 1
      }

      TextBlock {
        Text: "Press F12 for Dev Tools"
        HorizontalAlignment: Right
        Margin: 2
        Grid.ColumnProperty: 1
        Grid.RowProperty: 1
      }
    }
  }
}
