# Test Case: Verify Menu Item Display in TV and Mobile Preview

## Test Case ID
`E2E_001`

## Description
Verify that a newly added menu item is correctly displayed in both TV and Mobile preview views with the uploaded image.

## Test Type
End-to-End (E2E)

## Priority
High

## Author
Jakub Sobański

## Last Updated
2025-03-31

---

## Preconditions
- User has access to the Speeron Guest Portal with a valid token.
- Test image (`kot.jpg`) is available in the `/Images/` folder and marked as `Copy if newer` in project settings.
- Browser automation is configured using **Playwright + NUnit**.
- Headless mode is **disabled** (runs with UI for verification).

---

## Test Steps

1. Navigate to:  
   `https://reception.next-dev.speeron.com/recruitment-jakub-sobanski?token=<token>`

2. Wait for the page to load completely (network idle).

3. Open the menu editor view at:  
   `https://reception.next-dev.speeron.com/recruitment-jakub-sobanski/guest-portal/menu-editor`

4. Click the **"Add Menu Item"** button.

5. A modal dialog for creating a new menu item appears.

6. In the English tab, enter label: `test_001`.

7. Open the theme color dropdown.

8. Select the second color option from the list.

9. Upload `kot.jpg` as the **Tile Image**.

10. Confirm the upload in the modal dialog.

11. Upload the same `kot.jpg` as the **Background Image**.

12. Confirm this upload as well.

13. Click the **Polish** language tab.

14. Enter the same label: `test_001` in the Polish field.

15. Click the **"Add"** button to save the menu item.

16. Wait until the newly added item `test_001` is visible in the list.

17. Open the **Theme Editor** page at:  
    `https://reception.next-dev.speeron.com/recruitment-jakub-sobanski/guest-portal/theme-editor`

18. Switch to the **TV preview** iframe.

19. Verify that tile `test_001` exists and contains the uploaded image and background.

20. Switch to the **Mobile preview** iframe.

21. Assert that tile `test_001` is visible and includes both the tile image and background image.

---

## Expected Results

- The menu item `test_001` appears correctly in both **TV** and **Mobile** previews.
- The tile displays the uploaded **tile image**.
- The tile background is set using the uploaded **background image**.

---

## Postconditions

- The menu item `test_001` is deleted after test completion as part of the automated cleanup process.

---

## Tags
`e2e`, `UI`, `GuestPortal`, `Playwright`, `image-upload`, `tile-preview`

---

## Notes
- If the image file or token becomes invalid, update the test inputs accordingly.
- If the UI texts or DOM structure change, selectors and iframe IDs may require adjustment.
