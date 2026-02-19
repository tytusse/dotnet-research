#!/usr/bin/env fish

function info
    set_color green
    echo "# $argv"
    set_color normal
end

info "Podman version and machine info:"
podman version | grep -E "Version|Built|OS/Arch"
podman machine list 2>/dev/null || echo "No Podman machine (running native)"

info "Podman storage driver:"
podman info | grep -E "graphDriverName|GraphRoot"

info "Host mount namespace:"
ls -la /proc/self/ns/mnt

info "Creating tmpfs on host..."
mkdir -p ~/ramdisk
sudo mount -t tmpfs -o size=100M none ~/ramdisk
sudo chown $USER:$USER ~/ramdisk
echo "test-from-host" > ~/ramdisk/host-file.txt

info "Host sees:"
ls -la ~/ramdisk
mount | grep ramdisk

info "Starting container..."
podman run --rm -v ~/ramdisk:/ramdisk alpine:latest sh -c '
    echo "=== Inside container ==="
    mount | grep ramdisk
    ls -la /ramdisk
    echo "test-from-container" > /ramdisk/container-file.txt
    cat /proc/self/mountinfo | grep ramdisk
'

info "After container exits, host sees:"
ls -la ~/ramdisk

sudo umount ~/ramdisk
rm -rf ~/ramdisk