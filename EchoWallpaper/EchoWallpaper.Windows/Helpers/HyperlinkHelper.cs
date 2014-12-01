namespace U2UC.WinRT.HyperlinkBox.Helpers
{
    using System;
    using System.Text.RegularExpressions;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Documents;

    /// <summary>
    /// Ads auto-hyperlinking to a RichTextBlock control.
    /// </summary>
    public static class HyperlinkExtensions
    {
        /// <summary>
        /// The raw text property.
        /// </summary>
        public static readonly DependencyProperty RawTextProperty =
            DependencyProperty.RegisterAttached("RawText", typeof(string), typeof(RichTextBlock), new PropertyMetadata("", OnRawTextChanged));

        /// <summary>
        /// Gets the raw text.
        /// </summary>
        public static string GetRawText(DependencyObject obj)
        {
            return (string)obj.GetValue(RawTextProperty);
        }

        /// <summary>
        /// Sets the raw text.
        /// </summary>
        public static void SetRawText(DependencyObject obj, string value)
        {
            obj.SetValue(RawTextProperty, value);
        }

        /// <summary>
        /// Sets the raw text.
        /// </summary>
        /// <remarks>Extension method.</remarks>
        public static void AssignRawText(this RichTextBlock rtb, string value)
        {
            rtb.SetValue(RawTextProperty, value);
        }

        /// <summary>
        /// Called when raw text changed.
        /// </summary>
        private static void OnRawTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RichTextBlock rtb = (RichTextBlock)d;
            if (rtb != null)
            {
                // Working variables.
                string result = string.Empty;
                int index = 0;
                Paragraph par = new Paragraph();
                string text;
                Run run;
                Hyperlink link;
                string source = e.NewValue.ToString();

                // Regex initialization.
                string pattern = @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)";
                Regex regex = new Regex(pattern);
                var matches = regex.Matches(source);

                foreach (Match match in matches)
                {
                    // Add text before match.
                    int matchIndex = match.Index;
                    text = source.Substring(index, matchIndex - index);
                    run = new Run();
                    run.Text = text;
                    par.Inlines.Add(run);

                    // Add match as hyperlink.
                    string hyper = match.Value;
                    link = new Hyperlink();
                    run = new Run();
                    run.Text = hyper;
                    link.Inlines.Add(run);

                    // Complete link if necessary.
                    if (!hyper.Contains("@") && !hyper.StartsWith("http"))
                    {
                        hyper = @"http://" + hyper;
                    }

                    if (hyper.Contains("@") && !hyper.StartsWith("mailto"))
                    {
                        hyper = @"mailto://" + hyper;
                    }

                    link.NavigateUri = new Uri(hyper);
                    par.Inlines.Add(link);

                    index = matchIndex + match.Length;
                }

                // Add text after last match.
                text = source.Substring(index, source.Length - index);
                run = new Run();
                run.Text = text;
                par.Inlines.Add(run);

                // Update RichTextBlock content.
                rtb.Blocks.Clear();
                rtb.Blocks.Add(par);
            }
        }
    }
}
