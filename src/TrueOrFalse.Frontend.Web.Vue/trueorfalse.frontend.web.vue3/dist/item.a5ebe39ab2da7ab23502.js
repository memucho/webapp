(self["webpackChunkvue3_ssr_boilerplate"]=self["webpackChunkvue3_ssr_boilerplate"]||[]).push([[178],{5888:()=>{},3015:()=>{},1722:()=>{},788:(e,t,n)=>{"use strict";n.r(t),n.d(t,{default:()=>T,setup:()=>z});var m=n(569);const o={class:"item-view view"},s={class:"item-view-header"},i={class:"host"},r={class:"meta"},c={class:"item-view-comments"},l={class:"item-view-comments-header"},u={key:0,class:"comment-children"};function a(e,t,n,a,p,w){const d=(0,m.up)("router-link"),h=(0,m.up)("spinner"),v=(0,m.up)("comment");return(0,m.wg)(),(0,m.j4)("div",o,[(0,m.Wm)("div",s,[a.isAbsolute(a.item.url)?((0,m.wg)(),(0,m.j4)(m.HY,{key:0},[(0,m.Wm)("a",{href:a.item.url,target:"_blank",rel:"noopener"},[(0,m.Wm)("h1",{textContent:a.item.title},null,8,["textContent"])],8,["href"]),(0,m.Wm)("span",i,(0,m.zw)(e.itemUrl),1)],64)):((0,m.wg)(),(0,m.j4)("h1",{key:1,textContent:a.item.title},null,8,["textContent"])),(0,m.Wm)("p",r,[(0,m.Uk)((0,m.zw)(a.item.points)+" points | by ",1),(0,m.Wm)(d,{to:"/user/"+a.item.user},{default:(0,m.w5)((()=>[(0,m.Uk)((0,m.zw)(a.item.user),1)])),_:1},8,["to"]),(0,m.Uk)(" "+(0,m.zw)(a.item.time_ago),1)])]),(0,m.Wm)("div",c,[(0,m.Wm)("p",l,[(0,m.Uk)((0,m.zw)(a.item.comments?a.item.comments.length+" comments":" No comments yet")+" ",1),(0,m.Wm)(h,{show:a.item.loading},null,8,["show"])]),a.item.loading?(0,m.ry)("v-if",!0):((0,m.wg)(),(0,m.j4)("ul",u,[((0,m.wg)(!0),(0,m.j4)(m.HY,null,(0,m.Ko)(a.item.comments,(e=>((0,m.wg)(),(0,m.j4)(v,{key:e.id,comment:e},null,8,["comment"])))),128))]))])])}const p={key:0,class:"comment"},w={class:"by"},d={class:"comment-children"};function h(e,t,n,o,s,i){const r=(0,m.up)("router-link"),c=(0,m.up)("comment");return n.comment&&n.comment.user?((0,m.wg)(),(0,m.j4)("li",p,[(0,m.Wm)("div",w,[(0,m.Wm)(r,{to:"/user/"+n.comment.user},{default:(0,m.w5)((()=>[(0,m.Uk)((0,m.zw)(n.comment.user),1)])),_:1},8,["to"]),(0,m.Uk)(" "+(0,m.zw)(n.comment.time_ago),1)]),(0,m.Wm)("div",{class:"text",innerHTML:n.comment.content},null,8,["innerHTML"]),n.comment.comments&&n.comment.comments.length?((0,m.wg)(),(0,m.j4)("div",{key:0,class:[{open:o.open},"toggle"]},[(0,m.Wm)("a",{onClick:t[1]||(t[1]=e=>o.open=!o.open)},(0,m.zw)(o.open?"[-]":"[+] "+o.pluralize(n.comment.comments.length)+" collapsed"),1)],2)):(0,m.ry)("v-if",!0),(0,m.wy)((0,m.Wm)("ul",d,[((0,m.wg)(!0),(0,m.j4)(m.HY,null,(0,m.Ko)(n.comment.comments,(e=>((0,m.wg)(),(0,m.j4)(c,{key:e.id,comment:e},null,8,["comment"])))),128))],512),[[m.F8,o.open]])])):(0,m.ry)("v-if",!0)}function v(e){var t=(0,m.iH)(!0),n=function(e){return e+(1===e?" reply":" replies")};return{open:t,pluralize:n}}var g={name:"Comment",props:{comment:{type:Object,required:!0}}};g.setup=v;const f=g;n(5888);f.render=h;const k=f,y=(0,m.Wm)("circle",{class:"path",fill:"none","stroke-width":"4","stroke-linecap":"round",cx:"22",cy:"22",r:"20"},null,-1);function W(e,t,n,o,s,i){return(0,m.wg)(),(0,m.j4)(m.uT,null,{default:(0,m.w5)((()=>[(0,m.wy)(((0,m.wg)(),(0,m.j4)("svg",{class:[{show:n.show},"spinner"],width:"44px",height:"44px",viewBox:"0 0 44 44"},[y],2)),[[m.F8,n.show]])])),_:1})}const j={name:"Spinner",props:{show:{type:Boolean,required:!0}}};n(3015);j.render=W;const _=j;var b=n(675),C=n(2119);function z(){var e=(0,b.oR)(),t=(0,C.useRoute)();(0,m.bv)((function(){return e.dispatch("FETCH_ITEM",{id:t.params.id})}));var n=(0,m.Fl)((function(){return e.state.items[t.params.id]})),o=function(e){return/^https?:\/\//.test(e)};return{item:n,isAbsolute:o}}var x={name:"ItemView",components:{Comment:k,Spinner:_},serverPrefetch:function(){return this.$store.dispatch("FETCH_ITEM",{id:this.$route.params.id})}};x.setup=z;const H=x;n(1722);H.render=a;const T=H}}]);