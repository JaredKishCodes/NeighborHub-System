export function normalizeDisplayName(name?: string | null): string {
  if (!name?.trim()) {
    return '';
  }

  const parts = name.trim().split(/\s+/).filter(Boolean);
  if (parts.length === 2 && parts[0].toLowerCase() === parts[1].toLowerCase()) {
    return parts[0];
  }

  return name.trim();
}

export function formatDisplayName(
  firstName?: string,
  lastName?: string,
  fullName?: string | null
): string {
  const fromFull = normalizeDisplayName(fullName);
  if (fromFull) {
    return fromFull;
  }

  const first = firstName?.trim() ?? '';
  const last = lastName?.trim() ?? '';

  if (!first && !last) {
    return 'User';
  }

  if (!first) {
    return last;
  }

  if (!last) {
    return first;
  }

  if (first.toLowerCase() === last.toLowerCase()) {
    return first;
  }

  return `${first} ${last}`;
}
