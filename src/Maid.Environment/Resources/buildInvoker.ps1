# Reserved variables.
$BuildNumber    ??= '{{bNum}}';
$BuildVersion   ??= '{{bVer}}';
$BuildVersionId ??= '{{bVerId}}';

try {
    .'{{bScript}}' 
} catch {
    $_
    $host.SetShouldExit(-1);
}