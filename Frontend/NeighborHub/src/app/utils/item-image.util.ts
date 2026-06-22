export function resolveItemImageUrl(
  imageUrl: string | null | undefined,
  apiBaseUrl: string
): string {
  if (!imageUrl) {
    return '';
  }

  if (imageUrl.startsWith('data:') || /^https?:\/\//i.test(imageUrl)) {
    return imageUrl;
  }

  if (imageUrl.startsWith('/')) {
    return `${apiBaseUrl}${imageUrl}`;
  }

  return `${apiBaseUrl}/${imageUrl}`;
}
