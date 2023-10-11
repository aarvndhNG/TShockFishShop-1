# fishshop

The fish shop is a command shop. In addition to selling items, it also sells customized products such as summoning bosses, summoning invasions, generating NPCs, obtaining buffs, and adjusting time. Supports configuring purchase conditions and payment conditions.

download link:
| File name | Applicable version |
|---|---|
| [FishShop-v1.4.3.1.dll]([https://github.com/aarvndhNG/TShockFishShop-1/releases/download/v1.4.3/FishShop.dll])
<br>

## Commands

```
/fish list - View the shop.
/fish ask <item number> - Inquire about prices.
/fish buy <item number> [quantity] - Purchase items.

/fish info - Display fishing information.

/fish reload - Reload configurations.
/fish reset - Reset quantity records (1.4).

/fish special - View special commands (admins only).
/fish finish <number> - Modify the number of times you've completed your angler's tasks (admins only).
/fish change - Change today's angler's tasks (admins only).
/fish changesuper <item id|item name> - Specify today's angler's tasks (admins only).
/fish docs - Generate reference documentation (admins only).

/fish = /fishshop = /fs
```

<br>

## Permissions

| Permissions | Description |
|---|---|
| fishshop.change | Switch fishing tasks |
| fishshop.changesuper | Specify fishing tasks |
| fishshop.finish | Modify the number of fishing completions |
| fishshop.reload | Reload fish shop configuration and reset limit records |
| fishshop.special | For server owners and developers only |
| fishshop.ignore.allowgroup | Ignore user group purchase restrictions |

Authorization instructions (the server owner has all permissions by default):
```shell
/group addperm default fishshop.change
```

<br>

## Configuration instructions:

[https://www.yuque.com/hufang/bv/tshock-fish-shop](https://www.yuque.com/hufang/bv/tshock-fish-shop)

<br>

## support:
[https://afdian.net/@hufang360](https://afdian.net/@hufang360)
