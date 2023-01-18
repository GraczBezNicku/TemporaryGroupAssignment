# TemporaryGroupAssignment
A very simple plugin that lets you assign groups temporarly to players!
# Permissions
`PermissionsManagement` - needed to temporarly assign a group to someone.
# Example usage
`settempgroup 76561198315366910@steam moderator owner 10d`
This means that I will get the moderator rank for 10 days, and then it will automatically go back to owner.
# Default Config
```
temporary_gruop_assignment:
  is_enabled: true
  # How often should the plugin check if player's group expired (in seconds)
  check_interval: 30
```
