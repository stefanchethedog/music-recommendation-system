const BASE_URI = "http://localhost:5252";

const GET_ALL_USERS = `${BASE_URI}/users`;
const DELETE_USER = (id: String) => `${BASE_URI}/users/:id?id=${id}`
const GET_USER_BY_USERNAME = (username: String) =>
  `${BASE_URI}/users/getUserByUsername?username=${username}`;
const FOLLOW_USER = (id: String, username: String) =>
  `${BASE_URI}/users/Follow?id=${id}&username=${username}`;
const SUBSCRIBE_TO = (id: String, name: String) =>
  `${BASE_URI}/users/subscribe?id=${id}&name=${name}`;
const LIKE_SONG = (id: String, name: String) =>
  `${BASE_URI}/users/AddUserLikesSong?id=${id}&songName=${name}`;
const GET_RECOMMENDED_SONGS = (id: String) =>
  `${BASE_URI}/users/RecommendSongsByLikedSongs?id=${id}`;
const GET_LIKED_SONGS = (id: String) =>
  `${BASE_URI}/users/getLikedSongs?id=${id}`;
const CREATE_USER = `${BASE_URI}/users`;

const GET_ALL_ARTISTS = `${BASE_URI}/artists`;
const CREATE_ARTIST = `${BASE_URI}/artists`;
const DELETE_ARTIST = ( id: String ) => `${BASE_URI}/artists/:id?id=${id}`


const GET_ALL_GENRES = `${BASE_URI}/genres`
const CREATE_GENRE = `${BASE_URI}/genres`
const DELETE_GENRE = (id: String) => `${BASE_URI}/genres/:id?id=${id}`

const GET_SONGS = `${BASE_URI}/Songs/GetAll`
const CREATE_SONG = `${BASE_URI}/Songs/Create`
const DELETE_SONG = (id: String) => `${BASE_URI}/Songs/DeleteSong?id=${id}`

const GET_ALL_ALBUMS = `${BASE_URI}/albums`
const CREATE_ALBUM = `${BASE_URI}/albums`
const DELETE_ALBUM = (id: String) => `${BASE_URI}/albums/:id?id=${id}`
export {
  BASE_URI,
  GET_ALL_USERS,
  DELETE_USER,
  CREATE_USER,
  GET_USER_BY_USERNAME,
  FOLLOW_USER,
  SUBSCRIBE_TO,
  LIKE_SONG,
  GET_RECOMMENDED_SONGS,
  GET_LIKED_SONGS,
  GET_ALL_ARTISTS,
  CREATE_ARTIST,
  GET_ALL_GENRES,
  CREATE_GENRE,
  DELETE_ARTIST,
  DELETE_GENRE,

  GET_SONGS,
  CREATE_SONG,
  DELETE_SONG,

  GET_ALL_ALBUMS,
  CREATE_ALBUM,
  DELETE_ALBUM
};
