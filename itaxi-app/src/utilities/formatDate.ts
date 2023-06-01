function pad(s: number) {
    const padded = `0${s}`
    return padded.slice(-2)
}

export default function formatDate(iso: string, language: string) {
    const date = new Date(iso)
    const year = date.getFullYear()
    const month = pad(date.getMonth() + 1)
    const day = pad(date.getDate())
    const hours = pad(date.getHours())
    const minutes = pad(date.getMinutes())

    if (language === 'en-GB') {
        return `${year}-${month}-${day} ${hours}:${minutes}`
    }
    if (language === 'et') {
        return `${day}.${month}.${year} ${hours}:${minutes}`
    }
}