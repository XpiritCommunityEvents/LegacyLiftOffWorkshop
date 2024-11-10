#!/bin/sh

my_repo=$(gh repo view --json name -q ".name")
my_codespace=$(gh codespace list -R XpiritCommunityEvents/$my_repo -q '.[] | select( .state == "Available" )' --json name,state | jq -r '.name')

gh issue create --assignee roycornelissen,vriesmarcel --title $my_codespace --label "provisioning" --body "Please provision my codespace" -R XpiritCommunityEvents/LegacyLiftOffWorkshop

gh codespace ports visibility 7219:public --codespace $my_codespace