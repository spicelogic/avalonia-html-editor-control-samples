namespace SpiceLogic.Samples;

internal static class SampleDocuments
{
    internal const string ProjectBrief = """
        <!DOCTYPE html><html><head><title>Northfield project brief</title>
        <style>
        body { font-family:Segoe UI,Arial,sans-serif; color:#263449; padding:20px 30px; line-height:1.6; }
        h1 { color:#153c65; margin-bottom:8px; } h2 { color:#153c65; }
        .note { border-left:4px solid #008b8b; padding:12px 18px; background:#eef8f8; }
        table { border-collapse:collapse; width:100%; } th { background:#153c65; color:white; text-align:left; }
        td,th { padding:10px 14px; border:1px solid #d8e2ec; }
        .muted { color:#57697c; }
        </style></head><body>
        <p class="muted">Northfield Studio / Client delivery</p>
        <h1>A clearer path from brief to launch</h1>
        <p>Keep the project plan, review notes and client communication in one editable document.</p>
        <p class="note"><strong>Next milestone:</strong> Review the updated content and confirm the launch checklist.</p>
        <h2>Delivery plan</h2>
        <table><tr><th>Deliverable</th><th>Owner</th><th>Status</th></tr>
        <tr><td>Product overview</td><td>Content team</td><td>Ready for review</td></tr>
        <tr><td>Customer onboarding</td><td>Client services</td><td>In progress</td></tr></table>
        <p>Edit any part of this brief. Formatting, tables and Unicode text such as café and 日本語 stay in the exported HTML.</p>
        </body></html>
        """;
}
