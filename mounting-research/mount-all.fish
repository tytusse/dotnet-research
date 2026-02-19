#!/usr/bin/env fish

podman rm -f test-ramdisk

# set -f fish_trace on
# findmnt
mount | grep ram

function info
    set_color green
    echo "# $argv"
    set_color normal
end

info "Creating ~/ramdisk and mounting tmpfs there."
mkdir ~/ramdisk
sudo mount -t tmpfs none ~/ramdisk
sudo mount --make-shared ~/ramdisk
sudo chown $USER:$USER ~/ramdisk

info "Check host mount"
mount | grep /ramdisk

info "Create and start a container with ~/ramdisk:/ramdisk volume..."
podman create -v /home/$USER/ramdisk:/ramdisk --name test-ramdisk alpine:latest sleep infinity
podman start test-ramdisk

info "Check ramdisk mount inside container"
podman exec -it test-ramdisk mount | grep /ramdisk

info "Create /ramdisk/testfile.txt on volume inside container"
podman exec -it test-ramdisk touch /ramdisk/testfile.txt

info "ls /ramdisk inside container"
podman exec -it test-ramdisk ls /ramdisk

info "Remove container"
podman kill test-ramdisk
podman rm -f test-ramdisk

info "ls ~/ramdisk before unmounting:"
ls ~/ramdisk

info "Unmounting ~/ramdisk..."
sudo umount ~/ramdisk
or echo (set_color red)"Failed to unmount ~/ramdisk. Please check if it's still in use."

info "ls ~/ramdisk after unmounting:"
ls ~/ramdisk
rm -rf ~/ramdisk