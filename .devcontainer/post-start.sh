#!/bin/sh

my_codespace=$(gh codespace list -R XpiritCommunityEvents/LegacyLiftOffWorkshop --json name | jq -r '.[0].name')
gh issue create --assignee roycornelissen,vriesmarcel --title $my_codespace --label "provisioning" --body "Please provision my codespace" -R XpiritCommunityEvents/LegacyLiftOffWorkshop

# pull and start the HMS database
docker run -d -p 1433:1433 marcelv/project_hms_db

