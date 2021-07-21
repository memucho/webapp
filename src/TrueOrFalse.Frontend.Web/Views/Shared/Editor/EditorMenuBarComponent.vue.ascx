<div
    class="menubar is-hidden"
    :class="{ 'is-focused': focused }">

    <button
        class="menubar__button"
        :class="{ 'is-active':  editor.isActive('bold') }"
        @click="refocus()">
        <i class="fas fa-bold"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active':  editor.isActive('italic') }"
        @click="editor.chain().toggleItalic().focus().run()">
        <i class="fas fa-italic"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('strike') }"
        @click="editor.chain().toggleStrike().focus().run()">
        <i class="fas fa-strikethrough"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('underline') }"
        @click="editor.chain().toggleUnderline().focus().run()">
        <i class="fas fa-underline"></i>
    </button>

    <button
        v-if="heading"
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('heading', { level: 2})}"
        @click="editor.chain().toggleHeading({ level: 2 }).focus().run()">
        <b>
            H2

        </b>
    </button>

    <button
        v-if="heading"
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('heading', { level: 3 })}"
        @click="editor.chain().toggleHeading({ level: 3 }).focus().run()">
        <b>
            H3
        </b>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('bulletList') }"
        @click="editor.chain().focus().toggleBulletList().run()">
        <i class="fas fa-list-ul"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('orderedList') }"
        @click="editor.chain().focus().toggleOrderedList().run()">
        <i class="fas fa-list-ol"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('blockquote') }"
        @click="editor.chain().focus().toggleBlockquote().run()">
        <i class="fas fa-quote-right"></i>
    </button>

    <button
        class="menubar__button"
        :class="{ 'is-active': editor.isActive('codeBlock') }"
        @click="editor.chain().focus().toggleCodeBlock().run()">
        <i class="fas fa-file-code"></i>
    </button>

    <button
        class="menubar__button"
        @click="setLink"
        :class="{ 'is-active': editor.isActive('link') }">
        <i class="fas fa-link"></i>
    </button>
    
    <button
        v-if="editor.isActive('link')"
        class="menubar__button"
        @click="editor.chain().focus().unsetLink().run()">
        <i class="fas fa-unlink"></i>
    </button>

    <button
        class="menubar__button"
        @click="editor.chain().focus().setHorizontalRule().run()">
        <b>
            —
        </b>
    </button>

    <button
        class="menubar__button"
        @click="editor.chain().focus().undo().run()">
        <i class="fas fa-undo-alt"></i>
    </button>

    <button
        class="menubar__button"
        @click="editor.chain().focus().redo().run()">
        <i class="fas fa-redo-alt"></i>
    </button>

</div>