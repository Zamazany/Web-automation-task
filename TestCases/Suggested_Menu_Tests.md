# Suggested Automated Test Scenarios – Menu Tab

## Overview
The **Menu** tab allows hotel staff to manage the content that appears for guests on TV and mobile devices.
Below is a list of recommended automated test scenarios for this view, covering key aspects such as UI interaction, data integrity, and preview rendering.

All tests are designed to ensure that hotel employees can reliably manage guest-facing content without encountering functional or UX-related issues.

---

## Test Scenario 1: Add New Menu Item (E2E)
**Goal**: Verify that a new menu item can be added successfully and is visible in both TV and Mobile previews.  
**Why**: Core functionality – ensures staff can introduce new guest-facing options.  
**Test Type**: End-to-End  
**Status**: Implemented in project  
(See `E2E_001_Verify_Menu_Item_Display.md`)

---

## Test Scenario 2: Navigation Validation Across Menu Items
**Goal**: Ensure clicking different sections of the sidebar (Food & Drinks, Shops, etc.) navigates to the correct page.  
**Why**: Ensures intuitive navigation within the app.  
**Test Type**: End-to-End  
**Status**: Implemented in project  
(See `E2E_002_Verify_Navigation.md`)

---

## Test Scenario 3: Edit Existing Menu Item
**Goal**: Edit an existing menu item’s label, icon, visibility, or image, and verify the change is persisted.  
**Why**: Frequent real-life task for hotel staff; prevents outdated or incorrect data.  
**Approach**:
- Click on the Edit icon for an existing item
- Change fields (label, visibility, etc.)
- Save changes
- Assert updated values appear in the UI and preview

---

## Test Scenario 4: Delete Menu Item
**Goal**: Ensure that deleting a menu item removes it from the UI and previews.  
**Why**: Ensures staff can clean up or archive old content.  
**Approach**:
- Click Delete icon
- Confirm deletion
- Assert item disappears from list and previews

---

## Test Scenario 5: Visibility Filtering
**Goal**: Validate that selecting a filter (e.g. `Mobile`, `TV`, `Hidden`) updates the visible item list accordingly.  
**Why**: Important for targeting content to the right platform.  
**Approach**:
- Select a visibility filter from the dropdown
- Assert that only appropriate items are displayed

---

## Test Scenario 6: Language Switcher
**Goal**: Test switching between language tabs (e.g., English ⇄ Polish) updates visible labels.  
**Why**: Supports international usage and ensures localization is handled properly.  
**Approach**:
- Switch `Display Language` dropdown
- Assert that menu labels update accordingly

---

## Test Scenario 7: Drag and Drop / Reorder Menu Items
**Goal**: Ensure the "Reorder elements" feature allows drag-and-drop reordering of menu items.  
**Why**: Organizational flexibility for better UX.  
**Approach**:
- Click `Reorder elements`
- Drag one item below another
- Save
- Assert the new order persists on reload

---

## Test Scenario 8: Upload Image Validation
**Goal**: Upload unsupported or oversized images and validate proper error handling.  
**Why**: Prevents UI crashes or performance issues.  
**Approach**:
- Upload image >10MB or in invalid format
- Assert error message is displayed and image is not accepted

---

## Test Scenario 9: Role-based Access (Optional)
**Goal**: Ensure that restricted users cannot modify menu items (if RBAC exists).  
**Why**: Security and permission control.  
**Approach**:
- Log in as read-only user
- Attempt to add/edit/delete items
- Assert buttons are disabled or access is denied

---

## Summary

| Scenario                          | Importance | Implemented |
|----------------------------------|------------|-------------|
| Add New Menu Item                | High       | Yes         |
| Navigation Validation            | Medium     | Yes         |
| Edit Menu Item                   | High       | NO          |
| Delete Menu Item                 | Medium     | NO          |
| Visibility Filtering             | Medium     | NO          |
| Language Switching               | Medium     | NO          |
| Reorder Items                    | Low        | NO          |
| Upload Image Validation          | Medium     | NO          |
| Role-based Access Control        | Low        | NO          |

---

## Notes
- Mocking backend may be used for edit/delete validation if test data cannot be reset easily.
- Image upload and preview tests should be done in non-headless mode for visual confirmation.
- If tests will be scaled, consider adding API-based state resets for setup/teardown.

