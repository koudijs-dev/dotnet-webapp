This app is going to be run in the context of a k8s cluster with an ingress setup where auth hedaers are forwarden on all incomming calls

This is an example of the headers:
https://podinfo.k8s.koudijs.dev/headers

{
  "Accept": [
    "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7"
  ],
  "Accept-Encoding": [
    "gzip, deflate, br, zstd"
  ],
  "Accept-Language": [
    "nl-NL,nl;q=0.9,en-NL;q=0.8,en;q=0.7,en-US;q=0.6,fr;q=0.5"
  ],
  "Cookie": [
    "_oauth2_proxy=gtGRScF68baEdveNrUsCXOsYDyQSG0fM81OIrLgNVWYaoUAd0V_3i-GYhu8vuAzCLxsskPlIGcwDw3idDAEN6NBICN-tDXyBTMNwwV0fdNbaEgEqYq3T0kz7WwM6xe5eC0BZy5SAz2y90uPr5sAm-mmt45YLFJdhy4qcNlDxaPKUFdVFiDhzBuv3nORoG_1kkyQT7RFLXG6QJiGiMHMBy-shyMYWgZ_iK0upDjgjkUj8ZNFV3HDtwMox2opSf0JssU8FrRamX9tBdBaJNhNkxqmU_KBf8zyPbhTHXiWfcED24ui67Y5OBjQpicm2vaahHZgcoQFRAFLo_XQFZ6iTYxbTZNh84VIwMRimcHm9mMfh2UjHnQdDeTe41UxhuMB4vZbtlWToWqxyhl6G-NeG5jsG8zbwJ9lk-kEvA-R6aTxzBGL3AhUcsGmQO8pcaSHuPycDUUvWfMRA71huN8bm16zvK4xxtbYyn8UI3BXvh3DpHL-9dRmhQ60oTMZLor6dhrysUCcwDFuY55oWVg7YgYk_Y4aGdVfrIIgiGG6QVxsBHFN6eiQK2FZVoxvcXeECu9CobsxBAQrApZ_rxwPSO5bDB33EdqmQN_a1sCSriLmdpZzwcXo2vzoZIL0dPFJeC8kdUQBwyGpFkMXWpFPDcX8dG0r0QcOhoaBqUZ1oCcvLylpcuWSi8LonFMxWmvxTeIA30fNQUT7raVv0pGhO7LirohX4tdTFzBso4wRqBQyZzq-a3v2sFhY7lo8nfDZPradrPdfnyE6f1GXOdYgFuT21mQWYXZXNF1hCKROzGYOKuHwvSCVUCFsqrhOXHS6OJ69sKnnUPkCg_U1pSGFwDWlkH0VlIWcoiTqx5cLotsXWYO_veYe6Ge5AinCQkc6nSF6q_qMkBtNfGKJ4_Ea6TkEh7HfA44aY3VU2VI5z_G8wGzosaIaYmd-9XkVCDurBjMH2GOkucTV6NQkosL5iVdQNOKsU-SUOCpI-z3rx6EPLnbp6MXe2lKdP-BRZQM0pCPwAAsv4MFjdxmGMHzeK6j7hhvqGKVq8iOD1eWfR4EP1Eo8VO42Aowe7_iHKB7IFKrirTGAQv5r0QHK955bTnSmpxEZf-vlJHIvOT39vwUVKvwg_sBMemNbSux1wgw__RVtIbdV3JqwyNMovWa1JdiYUdEO8GiyIbpNclZF4K_COj90ve3EOA17lZ-rpKjJmY9bPatWKX-14I8HlEiLgRckHK5Cq0YldKw5qFeA3_xHlAs8dFwN9mogJp-b6UIdfVybhvesUp7wr_2JbHcpXuW1aQzV9P1N2Pb0CP1rw1R_fyqRtu9_gk_cLKP27QJKzPJI5yExxCDloa9S6tAXkQwreGxqIroVgk4vnMa8QgMLzLAxskleKef4WPEEFfQsPYF9JrWKRU39UrEUAAVusC5RX3Dit2ZlPrcieeL0VNRr5NjnxzD4kCKtrZII7rw3n5dYnBiRD_eaDxvJ1FsSLFye6CU5jjXdqDW-8FdqOHofetAUynAso1CXASYoEWXjTTffyCJD6YAdmTdFYk7SV7UjqjtGeYghYw-DKJYxt4t_Fb5FGgXGfU69rYa7G3NrBcPdQVZo2rvVpMn9bKZVXNEEj2VMNUpbTx4Ir_ufxBLh3hmmB4vShPATnEun5VUvB4OUEJn3jTUayo2hO207IS9x6AqJJ57uNwdpbsdEeKK4YLBllCld7wSWMPBHxuDFTFdTxICoctbUOadjZZJKAb9HbM6hx1PysVD7JQ9o4GHOs20IurPuWhYeMVin6LvfCkR7lJo13jOmeZ2KOsQPyEG-BA1n466fr7s6EUUeKpxtSXKVP6rOcQg7UyZUvBSk2cHxei6J-GHp1JomQkgf6AnSRBibi_-BEeWjLlTP7ATVvmYI9cpMRgYZSqXreTu3xS_dma9Onzk1EEpE1lan1y4MQ0750Jc5mHFVU19zvciDb4pyXsjBeoiym-K8m6kDP6u0K2mp881vlqNdrBoxvxALjsL1v-jmbP5tGUg7lMvJE-0PfGsVlZS-KACQmBqtmJJfKXclACKEi41ixx8_W8RZPJ1pNzPsLd7tM8zu1kK6xHvF0hlhQpvrF9cWFtMYR6XBYlnbikjV24XO_2b6tg3Ms_hVdyEEAg4QPgyQ3lLMUptwKv304REo8Itji_8nabwWC7H3nzfGF4q1SGVwdNt_j6Mdsfzw_bQ7QXzMD79J1hT9Fs1OxcfL94ECDG5BVDrDUdU2x_GFZkgNJTs4izsPzYKNp3Jy9kSu8WX7rm0Tp7tHKrw==|1782051637|Kifp0atTBBSxm18rBhNHe2teZD6a5eTvVRYy256t3Fg="
  ],
  "Priority": [
    "u=0, i"
  ],
  "Sec-Ch-Ua": [
    "\"Google Chrome\";v=\"149\", \"Chromium\";v=\"149\", \"Not)A;Brand\";v=\"24\""
  ],
  "Sec-Ch-Ua-Mobile": [
    "?0"
  ],
  "Sec-Ch-Ua-Platform": [
    "\"Windows\""
  ],
  "Sec-Fetch-Dest": [
    "document"
  ],
  "Sec-Fetch-Mode": [
    "navigate"
  ],
  "Sec-Fetch-Site": [
    "none"
  ],
  "Sec-Fetch-User": [
    "?1"
  ],
  "Upgrade-Insecure-Requests": [
    "1"
  ],
  "User-Agent": [
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36"
  ],
  "X-Api-Revision": [
    "6b7aab8a10d6ee8b895b0a5048f4ab0966ed29ff"
  ],
  "X-Api-Version": [
    "6.7.1"
  ],
  "X-Auth-Request-Email": [
    "pietassist@gmail.com"
  ],
  "X-Auth-Request-Groups": [
    "koudijs-dev:cloud-native-delivery-june-2026"
  ],
  "X-Auth-Request-User": [
    "CgkyNTc4NTk1NzESBmdpdGh1Yg"
  ],
  "X-Forwarded-For": [
    "86.90.30.197"
  ],
  "X-Forwarded-Host": [
    "podinfo.k8s.koudijs.dev"
  ],
  "X-Forwarded-Port": [
    "443"
  ],
  "X-Forwarded-Proto": [
    "https"
  ],
  "X-Forwarded-Server": [
    "traefik-lst64"
  ],
  "X-Real-Ip": [
    "86.90.30.197"
  ]
}