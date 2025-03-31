# Test Case: Verify Navigation Through Guest Portal Menu Options

## Test Case ID
`E2E_002`

## Description
Verify that navigating through each menu option in the Guest Portal using `data-test-id` attributes correctly redirects the user to the expected URL fragment that identifies each section.

## Test Type
End-to-End (E2E)

## Priority
Medium

## Author
Jakub Sobański

## Last Updated
2025-03-31

---

## Preconditions

- User has access to the Speeron Guest Portal with a valid token.
- Browser automation environment is properly configured with **Playwright + NUnit**.
- Token-based login is supported in the test environment.
- Headless mode is **disabled** to allow visual feedback during test execution.

---

## Test Steps

1. Navigate to the login URL:  
   `https://reception.next-dev.speeron.com/recruitment-jakub-sobanski?token=6M34foj4T45mCF2PGJv0t8ek7QWWpUN`

2. Wait until the page reaches a **network idle** state to ensure all dynamic resources are loaded.

3. Open the main Guest Portal view:  
   `https://reception.next-dev.speeron.com/recruitment-jakub-sobanski/guest-portal`

4. Assert that the current URL contains the substring: `guest-portal`

---

### Menu Navigation Validation

| Menu Label             | `data-test-id`                  | Fallback Text          | Expected URL Fragment      |
|------------------------|---------------------------------|------------------------|----------------------------|
| Food and Drinks        | `food-and-drinks-submenu`       | `Food and drinks`      | `food-and-drinks`          |
| Shops                  | `shops-submenu`                 | `Shops`                | `shops`                    |
| Services               | `services-submenu`              | `Services`             | `services`                 |
| Templates              | `templates-submenu`             | `Templates`            | `templates`                |
| Pages                  | `static-pages-submenu`          | `Pages`                | `static-pages`             |
| Menu                   | `menu-editor-submenu`           | `Menu`                 | `menu-editor`              |
| Orders                 | `orders-submenu`                | `Orders`               | `orders`                   |
| Statistics             | `statistics-submenu`            | `Statistics`           | `statistics`               |
| Theme Editor           | `theme-editor-submenu`          | `Theme editor`         | `theme-editor`             |

> For each menu option:
> - Attempt to click using the `data-test-id` attribute.
> - If not found, fallback to clicking by visible text.
> - After clicking, assert that the current URL contains the corresponding expected fragment.

---

## Expected Results

- The user is redirected to the correct section of the portal for each menu option.
- URLs contain the expected identifying fragments.
- No navigation or JavaScript errors are encountered during execution.
- All elements are accessible either by `data-test-id` or fallback text.

---

## Postconditions

- The user remains logged in after navigation.
- No data is changed during the process, and the environment remains clean for further testing.

---

## Tags

`e2e`, `GuestPortal`, `navigation`, `playwright`, `data-test-id`, `url-validation`

---

## Notes

- This test prioritizes `data-test-id` selectors for robustness and maintainability.
- Ensure selectors are stable and updated with any UI/DOM changes.
- Token should be refreshed if expired before executing the test.
